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
    public class ChoicesController : ApiController
    {
        private SnowflakeDataContext db = new SnowflakeDataContext();

        // GET: api/Choices
        public IEnumerable<ChoiceModel> GetChoices()
        {
            return Mapper.Map<IEnumerable<ChoiceModel>>(db.Choices);
        }

        // GET: api/Choices/5
        [ResponseType(typeof(ChoiceModel))]
        public IHttpActionResult GetChoice(int id)
        {
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<ChoiceModel>(choice));
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

            var dbChoice = db.Choices.Find(id);
            dbChoice.Update(modelChoice);
            db.Entry(dbChoice).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
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
        public IHttpActionResult PostChoice(ChoiceModel choice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newChoice = new Choice();
            newChoice.Update(choice);

            db.Choices.Add(newChoice);
            db.SaveChanges();

            choice.ChoiceId = newChoice.ChoiceId;

            return CreatedAtRoute("DefaultApi", new { id = choice.ChoiceId }, choice);
        }

        // DELETE: api/Choices/5
        [ResponseType(typeof(Choice))]
        public IHttpActionResult DeleteChoice(int id)
        {
            Choice choice = db.Choices.Find(id);
            if (choice == null)
            {
                return NotFound();
            }

            db.Choices.Remove(choice);
            db.SaveChanges();

            return Ok(Mapper.Map<ChoiceModel>(choice));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChoiceExists(int id)
        {
            return db.Choices.Count(e => e.ChoiceId == id) > 0;
        }
    }
}