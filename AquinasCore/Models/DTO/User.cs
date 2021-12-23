using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AquinasCore.Models.ORM;
namespace AquinasCore.Models.DTO
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Info { get; set; } = null;
        public string PfpUrl { get; set; } = null;
        //public ICollection<Group> Groups { get; set; } = new List<Group>();
        public User(Users user)
        {
            this.Id = user.inner_id;
            this.Username = user.username;
            this.Firstname = user.firstname;
            this.Lastname = user.lastname;
            this.Info = user.info;
            this.PfpUrl = user.pfp_url;
        }
    }
    public class UserReg
    {
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string info { get; set; } = null;
        public string pfpUrl { get; set; } = null;
    }
    public class UserViewer
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public UserViewer (long id, string username, string first, string last)
        {
            this.Id = id;
            this.Username = username;
            this.Firstname = first;
            this.Lastname = last;
        }
    }
}