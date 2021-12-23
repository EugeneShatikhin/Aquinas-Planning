using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AquinasCore.Models
{
    public class UserSelectedGroup
    {
        public long userid { get; set; }
        public long groupid { get; set; }
    }
    public static class InMemory
    {
        public static List<UserSelectedGroup> SelectedGroups = new List<UserSelectedGroup>();
    }
}