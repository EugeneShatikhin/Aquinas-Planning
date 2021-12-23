using System;
using System.Collections.Generic;
using BCrypt.Net;
using System.Linq;
using System.Web;
using AquinasCore.Models.ORM;
using AquinasCore.Models.DTO;
using AquinasCore.Models;
namespace AquinasCore.Controllers
{
    public class LoginObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public static class ValidateUser
    {
        //Return user ID if validated.Returns -1 if not found

        public static long Validate(HttpContext current)
        {
            try
            {
                var db = new DbGroup();
                var auth = current.Request.Headers["Authorization"].Split(';');
                var username = auth[0];
                var password = auth[1];
                Users users = db.Users.FirstOrDefault(x => x.username == username);
                if (users == null || !BCrypt.Net.BCrypt.Verify(password, users.password, false, HashType.SHA384))
                {
                    return -1;
                }
                return users.inner_id;
            }
            catch (Exception) { return -1; }
        }
        public static (long, long) ValidateUserGroup(HttpContext current)
        {
            var userid = Validate(current);
            if (userid < 0) return (-1, -1);
            var pair = InMemory.SelectedGroups.FirstOrDefault(x => x.userid == userid);
            if (pair == null) return (userid, -1);
            var db = new DbGroup();
            if (db.Users.FirstOrDefault(x => x.inner_id == userid).Groups.FirstOrDefault(x => x.group_id == pair.groupid) == null)
            {
                return (userid, -1);
            }
            return (userid, pair.groupid);
        }
    }
}