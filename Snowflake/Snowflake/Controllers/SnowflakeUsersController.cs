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
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork, ISnowflakeUserRepository userRepository) : base(userRepository)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Users
        public IEnumerable<SnowflakeUserModel> GetUsers()
        {
            return Mapper.Map<IEnumerable<SnowflakeUserModel>>(_snowflakeUserRepository.GetAll());
        }

        // GET: api/Users/5
        [ResponseType(typeof(SnowflakeUserModel))]
        public IHttpActionResult GetUser(int id)
        {
            SnowflakeUser User = _snowflakeUserRepository.GetById(id);

            if (User == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<SnowflakeUserModel>(User));
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, SnowflakeUserModel modelUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modelUser.Id)
            {
                return BadRequest();
            }

            var dbUser = _snowflakeUserRepository.GetById(id);
            dbUser.Update(modelUser);
            _snowflakeUserRepository.Update(dbUser);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(SnowflakeUserModel))]
        public IHttpActionResult PostUser(SnowflakeUserModel User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = new SnowflakeUser();
            newUser.Update(User);

            _snowflakeUserRepository.Add(newUser);
            _unitOfWork.Commit();

            User.Id = newUser.Id;

            return CreatedAtRoute("DefaultApi", new { id = User.Id }, User);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(SnowflakeUserModel))]
        public IHttpActionResult DeleteUser(int id)
        {
            SnowflakeUser User = _snowflakeUserRepository.GetById(id);
            if (User == null)
            {
                return NotFound();
            }

            _snowflakeUserRepository.Delete(User);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<SnowflakeUserModel>(User));
        }

        private bool UserExists(int id)
        {
            return _snowflakeUserRepository.Any(u => u.Id == id);
        }
    }
}