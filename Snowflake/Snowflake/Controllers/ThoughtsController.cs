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
    public class ThoughtsController : ApiController
    {
        private SnowflakeDataContext db = new SnowflakeDataContext();

        // GET: api/Thoughts
        public IEnumerable<ThoughtModel> GetThoughts()
        {
            return Mapper.Map<IEnumerable<ThoughtModel>>(db.Thoughts);
        }

        // GET: api/Thoughts/5
        [ResponseType(typeof(ThoughtModel))]
        public IHttpActionResult GetThought(int id)
        {
            Thought thought = db.Thoughts.Find(id);
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

            var dbThought = db.Thoughts.Find(id);
            dbThought.Update(modelThought);
            db.Entry(dbThought).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
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

            db.Thoughts.Add(newThought);
            db.SaveChanges();

            thought.ThoughtId = newThought.ThoughtId;

            return CreatedAtRoute("DefaultApi", new { id = thought.ThoughtId }, thought);
        }

        // DELETE: api/Thoughts/5
        [ResponseType(typeof(Thought))]
        public IHttpActionResult DeleteThought(int id)
        {
            Thought thought = db.Thoughts.Find(id);
            if (thought == null)
            {
                return NotFound();
            }

            db.Thoughts.Remove(thought);
            db.SaveChanges();

            return Ok(Mapper.Map<ThoughtModel>(thought));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ThoughtExists(int id)
        {
            return db.Thoughts.Count(e => e.ThoughtId == id) > 0;
        }
    }
}