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
    public class SnowflakeUsersController : ApiController
    {
        private SnowflakeDataContext db = new SnowflakeDataContext();

        // GET: api/SnowflakeUsers
        public IEnumerable<SnowflakeUserModel> GetSnowflakeUsers()
        {
            return Mapper.Map<IEnumerable<SnowflakeUserModel>>(db.Users);
        }

        // GET: api/SnowflakeUsers/5
        [ResponseType(typeof(SnowflakeUser))]
        public IHttpActionResult GetSnowflakeUser(int id)
        {
            SnowflakeUser snowflakeUser = db.Users.Find(id);
            if (snowflakeUser == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<SnowflakeUserModel>(User));
        }

        // PUT: api/SnowflakeUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSnowflakeUser(int id, SnowflakeUserModel modelSnowflakeUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modelSnowflakeUser.Id)
            {
                return BadRequest();
            }

            var dbSnowflakeUser = db.Users.Find(id);
            dbSnowflakeUser.Update(modelSnowflakeUser);
            db.Entry(dbSnowflakeUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SnowflakeUserExists(id))
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

        // POST: api/SnowflakeUsers
        [ResponseType(typeof(SnowflakeUser))]
        public IHttpActionResult PostSnowflakeUser(SnowflakeUserModel snowflakeUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newSnowflakeUser = new SnowflakeUser();
            newSnowflakeUser.Update(snowflakeUser);

            db.Users.Add(newSnowflakeUser);
            db.SaveChanges();

            snowflakeUser.Id = newSnowflakeUser.Id;

            return CreatedAtRoute("DefaultApi", new { id = snowflakeUser.Id }, snowflakeUser);
        }

        // DELETE: api/SnowflakeUsers/5
        [ResponseType(typeof(SnowflakeUser))]
        public IHttpActionResult DeleteSnowflakeUser(int id)
        {
            SnowflakeUser snowflakeUser = db.Users.Find(id);
            if (snowflakeUser == null)
            {
                return NotFound();
            }

            db.Users.Remove(snowflakeUser);
            db.SaveChanges();

            return Ok(Mapper.Map<SnowflakeUserModel>(snowflakeUser));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SnowflakeUserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}