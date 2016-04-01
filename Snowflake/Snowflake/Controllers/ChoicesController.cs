using AutoMapper;
using Snowflake.Core.Domain;
using Snowflake.Core.Infrastructure;
using Snowflake.Core.Models;
using Snowflake.Core.Repository;
using Snowflake.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Snowflake.Api.Controllers
{
    public class ChoicesController : BaseApiController
    {
        private readonly IChoiceRepository _ChoiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChoicesController(IChoiceRepository ChoiceRepository, IUnitOfWork unitOfWork, ISnowflakeUserRepository userRepository) : base(userRepository)
        {
            _ChoiceRepository = ChoiceRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Choices
        public IEnumerable<ChoiceModel> GetChoices()
        {
            return Mapper.Map<IEnumerable<ChoiceModel>>(_ChoiceRepository.GetAll());
        }

        // GET: api/Choices/5
        [ResponseType(typeof(ChoiceModel))]
        public IHttpActionResult GetChoice(int id)
        {
            Choice Choice = _ChoiceRepository.GetById(id);

            if (Choice == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<ChoiceModel>(Choice));
        }

        // PUT: api/Choices/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChoice(int id, ChoiceModel modelChoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modelChoice.ChoiceId)
            {
                return BadRequest();
            }

            var dbChoice = _ChoiceRepository.GetById(id);
            dbChoice.Update(modelChoice);
            _ChoiceRepository.Update(dbChoice);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!ChoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Choices
        [ResponseType(typeof(Choice))]
        public IHttpActionResult PostChoice(ChoiceModel Choice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newChoice = new Choice();
            newChoice.Update(Choice);

            _ChoiceRepository.Add(newChoice);
            _unitOfWork.Commit();

            Choice.ChoiceId = newChoice.ChoiceId;

            return CreatedAtRoute("DefaultApi", new { id = Choice.ChoiceId }, Choice);
        }

        // DELETE: api/Choices/5
        [ResponseType(typeof(Choice))]
        public IHttpActionResult DeleteChoice(int id)
        {
            Choice Choice = _ChoiceRepository.GetById(id);
            if (Choice == null)
            {
                return NotFound();
            }

            _ChoiceRepository.Delete(Choice);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<ChoiceModel>(Choice));
        }

        private bool ChoiceExists(int id)
        {
            return _ChoiceRepository.Any(r => r.ChoiceId == id);
        }
    }
}