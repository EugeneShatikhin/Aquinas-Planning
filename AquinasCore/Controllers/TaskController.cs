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
    [RoutePrefix("groups/group/task")]
    public class TaskController : ApiController
    {
        private DbGroup db = new DbGroup();

        [Route("new")]
        [HttpPost]
        public IHttpActionResult PostTask(TaskCreate task)
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;
            if (userid == -1 || groupid == -1) return Unauthorized();
            // check if user is allowed to make tasks
            if (!Perm.IsAllowed(userid, groupid, Perms.MakeTask)) return Unauthorized();
            var check = Task.GetOne(groupid, task.name);
            if (check != null) return BadRequest($"This task already exists at id = {check.task_id}");
            var newTask = new Task(task, userid);
            // для правильного отображения
            var taskid = Task.Create(groupid, newTask);
            newTask.task_id = taskid;
            return Ok(newTask);
        }
        [Route("view")]
        [HttpGet]
        public IHttpActionResult ViewTask(long taskid)
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;
            if (userid == -1 || groupid == -1) return Unauthorized();
            var task = new TaskViewer(groupid, taskid);
            return Ok(task);
        }
        [Route("list")]
        [HttpGet]
        public IHttpActionResult ListTasks()
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;
            if (userid == -1 || groupid == -1) return Unauthorized();
            var tasks = Task.Get(groupid);
            return Ok(tasks);
        }
        // ------------------------------------------------------------------------- //
        public class AssignUserClass
        {
            public long taskid { get; set; }
            public long[] users { get; set; }
        }
        public class AssignRoleClass
        {
            public long taskid { get; set; }
            public int[] roles { get; set; }
        }
        [Route("assign_users")]
        [HttpPost]
        public IHttpActionResult AssignUsers(AssignUserClass obj)
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;
            if (userid == -1 || groupid == -1) return Unauthorized();
            if (!Perm.IsAllowed(userid, groupid, Perms.AssignOthers)) return Unauthorized();
            var users = obj.users;
            var taskid = obj.taskid;
            foreach (var user in users)
            {
                Task.AssignUser(groupid, taskid, user);
            }
            return Ok("Successfully assigned users to task.");
        }
        [Route("assign_roles")]
        [HttpPost]
        public IHttpActionResult AssignRoles(AssignRoleClass obj)
        {
            var ids = ValidateUser.ValidateUserGroup(HttpContext.Current);
            var userid = ids.Item1;
            var groupid = ids.Item2;
            if (userid == -1 || groupid == -1) return Unauthorized();
            if (!Perm.IsAllowed(userid, groupid, Perms.AssignOthers)) return Unauthorized();
            var roles = obj.roles;
            var taskid = obj.taskid;
            foreach (var role in roles)
            {
                Task.AssignRole(groupid, taskid, role);
            }
            return Ok("Successfully assigned roles to task.");
        }
    }
}