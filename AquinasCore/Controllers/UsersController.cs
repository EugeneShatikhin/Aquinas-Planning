using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BCrypt.Net;
using AquinasCore.Models.ORM;
using AquinasCore.Models.DTO;
namespace AquinasCore.Controllers
{
    public class UsersController : ApiController
    {
        private DbGroup db = new DbGroup();
        [Route("login")]
        [HttpGet]
        [ResponseType(typeof(Users))]
        public IHttpActionResult GetUsers()
        {
            var userid = ValidateUser.Validate(HttpContext.Current);
            if (userid < 0)
            {
                return Unauthorized();
            }
            var user = db.Users.FirstOrDefault(x => x.inner_id == userid);
            var groups = user.Groups;
            if (!groups.Any())
            {
                return Ok("You don't have any groups. Create new? :wink:");
            }
            return Ok(JObject.FromObject(new { user, groups }));
        }
        [Route("register")]
        [HttpPost]
        [ResponseType(typeof(Users))]
        public IHttpActionResult PostUsers(UserReg users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = db.Users.FirstOrDefault(e => e.username == users.username);
            if (user != null)
            {
                return BadRequest("User with this username already exists.");
            }
            users.password = BCrypt.Net.BCrypt.HashPassword(users.password);
            var cur = new Users { username = users.username, password = users.password, firstname = users.firstname, lastname = users.lastname, info = users.info, pfp_url = users.pfpUrl };
            db.Users.Add(cur);
            db.SaveChanges();
            return Ok(db.Users.FirstOrDefault(u => u.username == cur.username));
        }

        //[Route("register")]
        //[HttpPost]
        //[ResponseType(typeof(Users))]
        //public IHttpActionResult PostUsers(Users users)
        //{
        //    return Ok(users);
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var user = db.Users.FirstOrDefault(e => e.username == users.username);
        //    if (user != null)
        //    {
        //        return BadRequest("User with this username already exists.");
        //    }
        //    users.password = BCrypt.Net.BCrypt.HashPassword(users.password);
        //    db.Users.Add(users);
        //    db.SaveChanges();
        //    return Ok(new User(users));
        //    //return CreatedAtRoute("DefaultApi", new { id = users.inner_id }, users);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsersExists(long id)
        {
            return db.Users.Count(e => e.inner_id == id) > 0;
        }
    }
}