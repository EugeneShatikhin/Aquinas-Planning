using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;
using Newtonsoft.Json;
using AquinasCore.Models.ORM;
using AquinasCore.Models.DTO;
using AquinasCore.Models;
using AquinasCore.SQL;
namespace AquinasCore.Controllers
{
    [RoutePrefix("groups")]
    public class GroupsController : ApiController
    {
        private DbGroup db = new DbGroup();

        [Route("list")]
        [HttpGet]
        [ResponseType(typeof(Groups))]
        public IHttpActionResult GetGroups()
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
            return Ok(groups);
        }

        [Route("new")]
        [HttpPost]
        [ResponseType(typeof(Groups))]
        public IHttpActionResult PostGroups(CreateGroup group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userid = ValidateUser.Validate(HttpContext.Current);
            if (userid < 0)
            {
                return Unauthorized();
            }
            if (db.Groups.FirstOrDefault(x => x.group_name == group.Name) != null)
            {
                return BadRequest("Group with this name already exists.");
            }
            var newGroup = new Groups
            {
                group_name = group.Name,
                group_description = group.Description,
                owner_id = userid,
                credentials = group.Credentials,
                current_price = 5
            };
            db.Groups.Add(newGroup);
            db.SaveChanges();
            var groupid = db.Groups.FirstOrDefault(x => x.group_name == group.Name).group_id;
            var run = new DbGroupCreator(groupid);
            Role.GiveOwnerRole(groupid, userid);
            return Ok(newGroup);
        }
        [Route("change")]
        [HttpPut]
        [ResponseType(typeof(Groups))]
        public IHttpActionResult ChangeGroup(long id)
        {
            var userid = ValidateUser.Validate(HttpContext.Current);
            if (userid < 0)
            {
                return Unauthorized();
            }
            var user = db.Users.FirstOrDefault(x => x.inner_id == userid);
            var group = user.Groups.FirstOrDefault(x => x.group_id == id);
            if (group == null)
            {
                return Unauthorized();
            }
            var pair = InMemory.SelectedGroups.FirstOrDefault(x => x.userid == userid);
            if (pair == null) InMemory.SelectedGroups.Add(new UserSelectedGroup { userid = userid, groupid = id });
            else pair.groupid = id;
            return Ok(new GroupViewer(group.group_id, userid));
            //return Ok("ok");
        }
        [Route("view")]
        [HttpGet]
        public IHttpActionResult View(DateTime? ShowTill = null)
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;
            if (userid == -1 || groupid == -1) return Unauthorized();
            // TODO EVERYTHING !!!
            var g = new GroupViewer(groupid, userid, ShowTill);
            return Ok(g);
        }
        [Route("invite")]
        [HttpPost]
        public IHttpActionResult Invite(string username)
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;
            if (userid == -1 || groupid == -1) return Unauthorized();
            if (!Perm.IsAllowed(userid, groupid, Perms.InviteUsers)) return Unauthorized();
            var invitee = db.Users.FirstOrDefault(u => u.username == username);
            if (invitee == null) return BadRequest($"Couldn't invite {username} to group since this user doesn't exist.");
            Group.Invite(groupid, invitee.inner_id);
            return Ok();
        }
        //--------------------------------------------------------------------------------//
        //------------------------------------ WARNING -----------------------------------//
        //------------ THIS ROUTE IS ABSOLUTELY ___NOT___ MEANT TO BE EXECUTED -----------//
        //--------------------- AND ONLY EXISTS FOR TESTING PURPOSES ---------------------//
        //------------------------ PLEASE, REFRAIN FROM USING IT. ------------------------//
        //--------------------------------------------------------------------------------//
        [Route("delete")]
        [HttpPost]
        [ResponseType(typeof(Groups))]
        public IHttpActionResult DeleteGroups(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userid = ValidateUser.Validate(HttpContext.Current);
            if (userid < 0)
            {
                return Unauthorized();
            }
            var group = db.Groups.FirstOrDefault(x => x.group_id == id);
            if (group == null)
            {
                return BadRequest("Group with this name doesn't exist.");
            }
            if (group.owner_id != userid)
            {
                return BadRequest("You don't have permissions to perform this action.");
            }
            var run = new DbGroupDeletor(id);
            return Ok("Oops, just got you sacked...");
        }
        //--------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------//
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupsExists(long id)
        {
            return db.Groups.Count(e => e.group_id == id) > 0;
        }
    }
}