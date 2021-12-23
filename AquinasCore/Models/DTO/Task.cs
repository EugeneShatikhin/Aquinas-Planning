using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AquinasCore.SQL;
namespace AquinasCore.Models.DTO
{
    public class TaskCreate
    {
        public string name { get; set; }
        public string description { get; set; }
        public DateTime date_deadline { get; set; }
    }
    public class Task
    {
        public long? task_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_deadline { get; set; }
        public long?author_id { get; set; }
        public Task (TaskCreate tc, long id)
        {
            this.name = tc.name;
            this.description = tc.description;
            this.date_deadline = tc.date_deadline;
            this.date_created = DateTime.Now;
            this.author_id = id;
        }
        public Task (long id, string name, string description, DateTime created, DateTime deadline, long author)
        {
            this.task_id = id;
            this.name = name;
            this.description = description;
            this.date_created = created;
            this.date_deadline = deadline;
            this.author_id = author;
        }
        public static long Create(long groupid, Task t)
        {
            string sql = $"INSERT INTO Tasks_{groupid} VALUES ('{t.name}', '{t.description}', '{t.date_created}', " +
                $"'{t.date_deadline}', {t.author_id})";
            DbWriter.Write(sql);
            var task = GetOne(groupid, t.name);
            sql = $"INSERT INTO Task_Users_{groupid} VALUES ({task.task_id}, {task.author_id})";
            DbWriter.Write(sql);
            return (long)task.task_id;
        }
        public static bool UserAssigned(long groupid, long taskid, long userid)
        {
            string sql = $"SELECT * FROM Task_Users_{groupid} WHERE task_id = {taskid} AND user_id = {userid}";
            if (DbReader.Read(sql).Rows.Count == 0) return false;
            return true;
        }
        public static bool RoleAssigned(long groupid, long taskid, long roleid)
        {
            string sql = $"SELECT * FROM Task_Roles_{groupid} WHERE task_id = {taskid} AND role_id = {roleid}";
            if (DbReader.Read(sql).Rows.Count == 0) return false;
            return true;
        }
        // this operation is safe - it will check if user is already assigned
        public static void AssignUser(long groupid, long taskid, long userid)
        {
            if (UserAssigned(groupid, taskid, userid)) return;
            string sql = $"INSERT INTO Task_Users_{groupid} VALUES ({taskid}, {userid})";
            DbWriter.Write(sql);
        }
        // this operation is safe - it will check if user is already assigned
        public static void AssignRole(long groupid, long taskid, int roleid)
        {
            if (RoleAssigned(groupid, taskid, roleid)) return;
            string sql = $"INSERT INTO Task_Roles_{groupid} VALUES ({taskid}, {roleid})";
            DbWriter.Write(sql);
        }
        public static Task GetOne(long groupid, long taskid)
        {
            var tasks = Get(groupid);
            return tasks.FirstOrDefault(t => t.task_id == taskid);
        }
        public static Task GetOne(long groupid, string name)
        {
            var tasks = Get(groupid);
            return tasks.FirstOrDefault(t => t.name == name);
        }
        public static List<Task> Get(long groupid)
        {
            string sql = $"SELECT * FROM Tasks_{groupid}";
            var result = DbReader.Read(sql);
            var task = new List<Task>();
            foreach (DataRow row in result.Rows)
            {
                task.Add(new Task((long)row[0], (string)row[1], (string)row[2], (DateTime)row[3], (DateTime)row[4],(long)row[5]));
            }
            return task;
        }
        public static List<UserViewer> GetUsers(long groupid, long taskid)
        {
            string sql = $"SELECT u.inner_id, u.username, u.firstname, u.lastname FROM Task_Users_{groupid} tu " +
                $"JOIN Users u ON tu.user_id = u.inner_id WHERE tu.task_id = {taskid}";
            var result = DbReader.Read(sql);
            var uv = new List<UserViewer>();
            foreach (DataRow row in result.Rows)
            {
                uv.Add(new UserViewer((long)row[0], (string)row[1], (string)row[2], (string)row[3]));
            }
            return uv;
        }
        public static List<RoleViewer> GetRoles(long groupid, long taskid)
        {
            string sql = $"SELECT u.id, u.name FROM Task_Roles_{groupid} tr " +
                $"JOIN Roles_{groupid} u ON tr.role_id = u.id WHERE tr.task_id = {taskid}";
            var result = DbReader.Read(sql);
            var rv = new List<RoleViewer>();
            foreach (DataRow row in result.Rows)
            {
                rv.Add(new RoleViewer((int)row[0], (string)row[1]));
            }
            return rv;
        }
    }
    public class TaskViewer
    {
        public long task_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_deadline { get; set; }
        public long author_id { get; set; }
        public List<UserViewer> users { get; set; }
        public List<RoleViewer> roles { get; set; }
        // construct task viewer
        public TaskViewer (long groupid, long taskid)
        {
            this.task_id = taskid;
            var task = Task.GetOne(groupid, taskid);
            this.name = task.name;
            this.description = task.description;
            this.date_created = task.date_created;
            this.date_deadline = task.date_deadline;
            this.author_id = (long)task.author_id;
            this.users = Task.GetUsers(groupid, taskid);
            this.roles = Task.GetRoles(groupid, taskid);
        }
    }
    public class MicroTaskViewer
    {
        public long task_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime date_deadline { get; set; }
        public MicroTaskViewer(long groupid, long taskid)
        {
            this.task_id = taskid;
            var task = Task.GetOne(groupid, taskid);
            this.name = task.name;
            this.description = task.description;
            this.date_deadline = task.date_deadline;
        }
        public MicroTaskViewer(TaskViewer task)
        {
            this.task_id = task.task_id;
            this.name = task.name;
            this.description = task.description;
            this.date_deadline = task.date_deadline;
        }
    }
}