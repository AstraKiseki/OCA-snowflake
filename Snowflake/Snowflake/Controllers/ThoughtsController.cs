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
    public class ThoughtsController : BaseApiController
    {
        private readonly IThoughtRepository _thoughtRepository;
        private readonly IChoiceRepository _choiceRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ThoughtsController(IThoughtRepository thoughtRepository, IConversationRepository conversationRepository, IChoiceRepository choiceRepository, IUnitOfWork unitOfWork, ISnowflakeUserRepository userRepository) : base(userRepository)
        {
            _thoughtRepository = thoughtRepository;
            _choiceRepository = choiceRepository;
            _conversationRepository = conversationRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Thoughts
        public IEnumerable<ThoughtModel> GetThoughts()
        {
            return Mapper.Map<IEnumerable<ThoughtModel>>(
                _thoughtRepository.GetWhere(t => t.UserId != CurrentUser.Id && t.Choices.All(c => c.UserId != CurrentUser.Id))
            );
        }

        // GET: api/Thoughts/5
        [ResponseType(typeof(ThoughtModel))]
        public IHttpActionResult GetThought(int id)
        {
            Thought thought = _thoughtRepository.GetById(id);

            if (thought == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<ThoughtModel>(thought));
        }

        // PUT: api/Thoughts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutThought(int id, ThoughtModel modelThought)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modelThought.ThoughtId)
            {
                return BadRequest();
            }

            var dbThought = _thoughtRepository.GetById(id);
            dbThought.Update(modelThought);
            _thoughtRepository.Update(dbThought);

            try
            {
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                if (!ThoughtExists(id))
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

        // POST: api/Thoughts
        [ResponseType(typeof(Thought))]
        public IHttpActionResult PostThought(ThoughtModel thought)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newThought = new Thought();
            newThought.Update(thought);
            newThought.User = CurrentUser;

            _thoughtRepository.Add(newThought);
            _unitOfWork.Commit();

            thought.ThoughtId = newThought.ThoughtId;

            return CreatedAtRoute("DefaultApi", new { id = thought.ThoughtId }, thought);
        }

        // DELETE: api/Thoughts/5
        [ResponseType(typeof(Thought))]
        public IHttpActionResult DeleteThought(int id)
        {
            Thought thought = _thoughtRepository.GetById(id);
            if (thought == null)
            {
                return NotFound();
            }

            _thoughtRepository.Delete(thought);
            _unitOfWork.Commit();

            return Ok(Mapper.Map<ThoughtModel>(thought));
        }

        [Route("api/thoughts/{thoughtId}/like")]
        public IHttpActionResult LikeThought(int thoughtId)
        {
            var choice = new Choice
            {
                ThoughtId = thoughtId,
                UserId = CurrentUser.Id,
                Chosen = true 
            };

            _choiceRepository.Add(choice);

            _unitOfWork.Commit();

            var thought = _thoughtRepository.GetById(thoughtId);

            if(CurrentUser.Thoughts.Any(t => t.Choices.Any(c => c.UserId == thought.UserId)))
            {
                var conversation = new Conversation();

                conversation.Participations.Add(new Participation { User = CurrentUser });
                conversation.Participations.Add(new Participation { User = choice.Thought.User });

                _conversationRepository.Add(conversation);

                _unitOfWork.Commit();
            }

            return Ok();
        }

        [Route("api/thoughts/{thoughtId}/dislike")]
        public IHttpActionResult DislikeThought(int thoughtId)
        {
            var choice = new Choice
            {
                ThoughtId = thoughtId,
                UserId = CurrentUser.Id,
                Chosen = false
            };

            _choiceRepository.Add(choice);

            _unitOfWork.Commit();

            return Ok();
        }

        private bool ThoughtExists(int id)
        {
            return _thoughtRepository.Any(r => r.ThoughtId == id);
        }
    }
}