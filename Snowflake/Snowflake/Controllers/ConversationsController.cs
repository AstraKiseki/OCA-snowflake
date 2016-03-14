using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Snowflake.Core.Domain;
using Snowflake.Data.Infrastructure;
using AutoMapper;
using Snowflake.Core.Models;

namespace Snowflake.Api.Controllers
{
    public class ConversationsController : ApiController
    {
        private SnowflakeDataContext db = new SnowflakeDataContext();

        // GET: api/Conversations
        public IEnumerable<ConversationModel> GetConversations()
        {
            return Mapper.Map<IEnumerable<ConversationModel>>(db.Conversations);
        }

        // GET: api/Conversations/5
        [ResponseType(typeof(ConversationModel))]
        public IHttpActionResult GetConversation(int id)
        {
            Conversation conversation = db.Conversations.Find(id);
            if (conversation == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<ConversationModel>(conversation));
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
            var dbConversation = db.Conversations.Find(id);
            dbConversation.Update(modelConversation);
            db.Entry(dbConversation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
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
        public IHttpActionResult PostConversation(ConversationModel conversation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newConversation = new Conversation();
            newConversation.Update(conversation);

            db.Conversations.Add(newConversation);
            db.SaveChanges();

            conversation.ConversationId = newConversation.ConversationId;

            return CreatedAtRoute("DefaultApi", new { id = conversation.ConversationId }, conversation);
        }

        // DELETE: api/Conversations/5
        [ResponseType(typeof(Conversation))]
        public IHttpActionResult DeleteConversation(int id)
        {
            Conversation conversation = db.Conversations.Find(id);
            if (conversation == null)
            {
                return NotFound();
            }

            db.Conversations.Remove(conversation);
            db.SaveChanges();

            return Ok(Mapper.Map<ConversationModel>(conversation));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConversationExists(int id)
        {
            return db.Conversations.Count(e => e.ConversationId == id) > 0;
        }
    }
}