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
    public class MessagesController : BaseApiController
    {
        private readonly IMessageRepository _MessageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MessagesController(IMessageRepository MessageRepository, IUnitOfWork unitOfWork, ISnowflakeUserRepository userRepository) : base(userRepository)
        {
            _MessageRepository = MessageRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Messages
        public IEnumerable<MessageModel> GetMessages()
        {
            return Mapper.Map<IEnumerable<MessageModel>>(_MessageRepository.GetAll());
        }

        // GET: api/Messages/5
        [ResponseType(typeof(MessageModel))]
        public IHttpActionResult GetMessage(int id)
        {
            Message Message = _MessageRepository.GetById(id);

            if (Message == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<MessageModel>(Message));
        }

        // PUT: api/Messages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMessage(int id, MessageModel modelMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modelMessage.MessageId)
            {
                return BadRequest();
            }

            var dbMessage = _MessageRepository.GetById(id);
            dbMessage.Update(modelMessage);
            _MessageRepository.Update(dbMessage);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!MessageExists(id))
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

        // POST: api/Messages
        [ResponseType(typeof(Message))]
        public IHttpActionResult PostMessage(MessageModel Message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newMessage = new Message();
            newMessage.Update(Message);
            newMessage.User = CurrentUser;

            _MessageRepository.Add(newMessage);
            _unitOfWork.Commit();

            Message.MessageId = newMessage.MessageId;

            return CreatedAtRoute("DefaultApi", new { id = Message.MessageId }, Message);
        }

        // DELETE: api/Messages/5
        [ResponseType(typeof(Message))]
        public IHttpActionResult DeleteMessage(int id)
        {
            Message Message = _MessageRepository.GetById(id);
            if (Message == null)
            {
                return NotFound();
            }

            _MessageRepository.Delete(Message);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<MessageModel>(Message));
        }

        private bool MessageExists(int id)
        {
            return _MessageRepository.Any(r => r.MessageId == id);
        }
    }
}