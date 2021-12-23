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
    [RoutePrefix("groups/group/role")]
    public class RoleController : ApiController
    {
        private DbGroup db = new DbGroup();

        [Route("new")]
        [HttpPost]
        [ResponseType(typeof(Role))]
        public IHttpActionResult PostRoles(Role role)
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;    
            if (userid == -1 || groupid == -1) return Unauthorized();
            // check if user is allowed to make roles - if one of his roles has admin privileges
            if (!Perm.IsAllowed(userid, groupid, Perms.isAdmin)) return Unauthorized();
            // для правильного отображения
            var check = Role.GetOne(groupid, role.name);
            if (check != null) return BadRequest($"This role already exists at id = {check.Id}");
            var roleid = Role.Create(groupid, role);
            role.Id = roleid;
            return Ok(role);
        }
        [Route("list")]
        [HttpGet]
        [ResponseType(typeof(Role))]
        public IHttpActionResult ListRoles()
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;
            if (userid == -1 || groupid == -1) return Unauthorized();
            var roles = Role.Get(groupid);
            return Ok(roles);
        }
        [Route("assign")]
        [HttpPost]
        public IHttpActionResult AssignRole(long assignid, long roleid)
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;
            if (userid == -1 || groupid == -1) return Unauthorized();
            // check if user is allowed to assign roles
            if (!Perm.IsAllowed(userid, groupid, Perms.isAdmin)) return Unauthorized();
            // check if target user even is in this group
            var assignee = db.Users.FirstOrDefault(u => u.inner_id == assignid);
            if (assignee.Groups.FirstOrDefault(g => g.group_id == groupid) == null)
            {
                return BadRequest("Role couldn't be assigned since this user does not belong to this group.");
            }
            var rolename = Role.GetRoleName(groupid, roleid);
            return Ok($"Assigned role {rolename} to {assignee.username}.");
        }
    }
}