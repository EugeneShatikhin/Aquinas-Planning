using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using AquinasCore.SQL;
namespace AquinasCore.Models.DTO
{
    public enum Perms
    {
        isAdmin = 0,
        InviteUsers = 1,
        MakeTask = 2,
        CloseTask = 3,
        SelfAssign = 4,
        AssignOthers = 5,
        CreateTag = 6,
        AssignTag = 7,
        Comment = 8
    }
    // this class is for returning it in task requests
    public class RoleViewer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RoleViewer (int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
    public class Role
    {
        public int Id { get; set; } = 0;
        public string name { get; set; }
        public int weight { get; set; }
        public bool isAdmin { get; set; }
        public bool InviteUsers { get; set; }
        public bool MakeTask { get; set; }
        public bool CloseTask { get; set; }
        public bool SelfAssign { get; set; }
        public bool AssignOthers { get; set; }
        public bool CreateTag { get; set; }
        public bool AssignTag { get; set; }
        public bool Comment { get; set; }
        public Role() { }
        public Role(int id, string name, int weight, bool isAdmin, bool InviteUsers, bool MakeTask, bool CloseTask,
            bool SelfAssign, bool AssignOthers, bool CreateTag, bool AssignTag, bool Comment)
        {
            this.Id = id;
            this.name = name;
            this.weight = weight;
            this.isAdmin = isAdmin;
            this.InviteUsers = InviteUsers;
            this.MakeTask = MakeTask;
            this.CloseTask = CloseTask;
            this.SelfAssign = SelfAssign;
            this.AssignOthers = AssignOthers;
            this.CreateTag = CreateTag;
            this.AssignTag = AssignTag;
            this.Comment = Comment;
        }
        public static void GiveOwnerRole(long groupid, long adminid)
        {
            string sql = $"INSERT INTO User_Role_{groupid} VALUES ({adminid}, 2)";
            DbWriter.Write(sql);
        }
        public static int Create(long groupid, Role r)
        {
            string sql = $"INSERT INTO Roles_{groupid} VALUES ('{r.name}', {r.weight}, {(r.isAdmin ? 1 : 0)}, " +
                $"{(r.InviteUsers ? 1 : 0)}, {(r.MakeTask ? 1 : 0)}, {(r.CloseTask ? 1 : 0)}, {(r.SelfAssign ? 1 : 0)}, " +
                $"{(r.AssignOthers ? 1 : 0)}, {(r.CreateTag ? 1 : 0)}, {(r.AssignTag ? 1 : 0)}, {(r.Comment ? 1 : 0)})";
            DbWriter.Write(sql);
            var role = GetOne(groupid, r.name);
            return role.Id;
        }
        public static Role GetOne(long groupid, string name)
        {
            var role = Get(groupid);
            return role.FirstOrDefault(t => t.name == name);
        }
        public static List<Role> Get(long groupid)
        {
            string sql = $"SELECT * FROM Roles_{groupid}";
            var result = DbReader.Read(sql);
            var roles = new List<Role>();
            foreach (DataRow row in result.Rows)
            {
                roles.Add(new Role((int)row[0], (string)row[1], (int)row[2], (bool)row[3], (bool)row[4], (bool)row[5],
                    (bool)row[6], (bool)row[7], (bool)row[8], (bool)row[9], (bool)row[10], (bool)row[11]));
            }
            return roles;
        }
        public static bool IsAssigned(long assignid, long roleid, long groupid)
        {
            string sql = $"SELECT * FROM User_Role_{groupid} WHERE inner_id = {assignid} AND role_id = {roleid}";
            if (DbReader.Read(sql).Rows.Count == 0) return false;
            return true;
        }
        public static string GetRoleName(long groupid, long roleid)
        {
            string sql = $"SELECT name FROM Roles_{groupid} WHERE id = {roleid}";
            var role = DbReader.Read(sql);
            return role.Rows[0][0].ToString();
        }
        public static void Assign(long assignid, long roleid, long groupid)
        {
            if (IsAssigned(assignid, roleid, groupid)) return;
            string sql = $"INSERT INTO User_Role_{groupid} VALUES ({assignid}, {roleid})";
            DbWriter.Write(sql);
        }
    }
    public static class Perm
    {
        public static bool IsAllowed(long userid, long groupid, Perms perm)
        {
            string permission = perm.ToString();
            if (perm != Perms.isAdmin) permission = $"isAdmin, {perm}";
            string sql = $"SELECT isAdmin, {permission} FROM Roles_{groupid} r JOIN User_Role_{groupid} ur ON ur.role_id = r.id WHERE ur.inner_id = {userid}";
            var result = DbReader.Read(sql);
            foreach (DataRow row in result.Rows)
            {
                if ((bool)row[0] || (bool)row[1]) return true;
            }
            return false;
        }
    }
}