using AutoMapper;
using Snowflake.Core.Domain;
using Snowflake.Core.Infrastructure;
using Snowflake.Core.Models;
using Snowflake.Core.Repository;
using Snowflake.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Snowflake.Api.Controllers
{
    public class ConversationsController : BaseApiController
    {
        private readonly IConversationRepository _ConversationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConversationsController(IConversationRepository ConversationRepository, IUnitOfWork unitOfWork, ISnowflakeUserRepository userRepository) : base(userRepository)
        {
            _ConversationRepository = ConversationRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Conversations
        public IEnumerable<ConversationModel> GetConversations()
        {
            return Mapper.Map<IEnumerable<ConversationModel>>(_ConversationRepository.GetWhere(c => c.Participations.Any(p => p.UserId == CurrentUser.Id)));
        }

        [Route("api/conversations/{conversationId}/messages")]
        public IEnumerable<MessageModel> GetMessagesForConversation(int conversationId)
        {
            return Mapper.Map<IEnumerable<MessageModel>>(
                _ConversationRepository.GetById(conversationId).Messages
            );
        }

        // GET: api/Conversations/5
        [ResponseType(typeof(ConversationModel))]
        public IHttpActionResult GetConversation(int id)
        {
            Conversation Conversation = _ConversationRepository.GetById(id);

            if (Conversation == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<ConversationModel>(Conversation));
        }

        // PUT: api/Conversations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConversation(int id, ConversationModel modelConversation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modelConversation.ConversationId)
            {
                return BadRequest();
            }

            var dbConversation = _ConversationRepository.GetById(id);
            dbConversation.Update(modelConversation);
            _ConversationRepository.Update(dbConversation);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!ConversationExists(id))
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

        // POST: api/Conversations
        [ResponseType(typeof(Conversation))]
        public IHttpActionResult PostConversation(ConversationModel Conversation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newConversation = new Conversation();
            newConversation.Update(Conversation);

            _ConversationRepository.Add(newConversation);
            _unitOfWork.Commit();

            Conversation.ConversationId = newConversation.ConversationId;

            return CreatedAtRoute("DefaultApi", new { id = Conversation.ConversationId }, Conversation);
        }

        // DELETE: api/Conversations/5
        [ResponseType(typeof(Conversation))]
        public IHttpActionResult DeleteConversation(int id)
        {
            Conversation Conversation = _ConversationRepository.GetById(id);
            if (Conversation == null)
            {
                return NotFound();
            }

            _ConversationRepository.Delete(Conversation);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<ConversationModel>(Conversation));
        }

        private bool ConversationExists(int id)
        {
            return _ConversationRepository.Any(r => r.ConversationId == id);
        }
    }
}