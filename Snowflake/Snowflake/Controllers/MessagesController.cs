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
using Snowflake.Core.Models;
using AutoMapper;

namespace Snowflake.Api.Controllers
{
    public class MessagesController : ApiController
    {
        private SnowflakeDataContext db = new SnowflakeDataContext();

        // GET: api/Messages
        public IEnumerable<MessageModel> GetMessages()
        {
            return Mapper.Map<IEnumerable<MessageModel>>(db.Messages);
        }

        // GET: api/Messages/5
        [ResponseType(typeof(MessageModel))]
        public IHttpActionResult GetMessage(int id)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<MessageModel>(message));
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

            var dbMessage = db.Messages.Find(id);
            dbMessage.Update(modelMessage);
            db.Entry(dbMessage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
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
        public IHttpActionResult PostMessage(MessageModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newMessage = new Message();
            newMessage.Update(message);

            db.Messages.Add(newMessage);
            db.SaveChanges();

            message.MessageId = newMessage.MessageId;

            return CreatedAtRoute("DefaultApi", new { id = message.MessageId }, message);
        }

        // DELETE: api/Messages/5
        [ResponseType(typeof(Message))]
        public IHttpActionResult DeleteMessage(int id)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            db.Messages.Remove(message);
            db.SaveChanges();

            return Ok(Mapper.Map<MessageModel>(message));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return db.Messages.Count(e => e.MessageId == id) > 0;
        }
    }
}