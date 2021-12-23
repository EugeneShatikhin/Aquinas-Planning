using System;
using System.Data;
using AquinasCore.SQL;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AquinasCore.Models.ORM;
namespace AquinasCore.Models.DTO
{
    public class ViewGroup
    {
        public long Id { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public ViewGroup(Groups group)
        {
            this.Id = group.group_id;
            this.GroupName = group.group_name;
            this.GroupDescription = group.group_description;
        }
    }
    public class CreateGroup
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Credentials { get; set; }
    }
    public class Group
    {
        public static void Invite (long groupid, long userid)
        {
            string sql = $"INSERT INTO User_Groups VALUES({userid}, {groupid}); INSERT INTO User_Role_{groupid} VALUES ({userid}, 1)";
            DbWriter.Write(sql);
        }
        public static List<long> GetTasksId(long groupid)
        {
            string sql = $"SELECT task_id FROM Tasks_{groupid} ORDER BY task_id ASC";
            var result = DbReader.Read(sql);
            var tids = new List<long>();
            foreach (DataRow row in result.Rows)
            {
                tids.Add((long)row[0]);
            }
            return tids;
        }
        public static List<UserViewer> GetUsers(long groupid)
        {
            string sql = $"SELECT u.inner_id, u.username, u.firstname, u.lastname FROM User_Groups ug JOIN Users u " +
                $"ON ug.user_id = u.inner_id WHERE ug.group_id = {groupid}";
            var result = DbReader.Read(sql);
            var uv = new List<UserViewer>();
            foreach (DataRow row in result.Rows)
            {
                uv.Add(new UserViewer((long)row[0], (string)row[1], (string)row[2], (string)row[3]));
            }
            return uv;
        }
    }
    public class GroupViewer
    {
        public long GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserViewer> users { get; set; }
        public DateTime ShowingTill { get; set; } = DateTime.Today.AddMonths(1);
        public List<TaskViewer> tasks { get; set; }
        public List<MicroTaskViewer> ResponsibleFor { get; set; }
        public GroupViewer(long groupid, long userid, DateTime? showingTill = null)
        {
            this.GroupId = groupid;
            var group = new DbGroup().Groups.FirstOrDefault(g => g.group_id == groupid);
            this.Name = group.group_name;
            this.Description = group.group_description;
            this.ShowingTill = (showingTill == null ? this.ShowingTill : (DateTime)showingTill);
            this.users = Group.GetUsers(groupid);
            this.tasks = new List<TaskViewer>();
            this.ResponsibleFor = new List<MicroTaskViewer>();
            TaskViewer single;
            bool shouldAdd;
            foreach (var tId in Group.GetTasksId(groupid))
            {
                shouldAdd = false;
                single = new TaskViewer(groupid, tId);
                if (DateTime.Compare(single.date_deadline, this.ShowingTill) > 0) continue;
                this.tasks.Add(single);
                foreach (var user in single.users)
                {
                    if (userid == user.Id) { shouldAdd = true; break; }
                }
                if (!shouldAdd)
                {
                    foreach (var role in single.roles)
                    {
                        if (Role.IsAssigned(userid, role.Id, groupid)) { shouldAdd = true; break; }
                    }
                }
                if(shouldAdd) this.ResponsibleFor.Add(new MicroTaskViewer(groupid, tId));
            }
        }
    }
}