using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
namespace AquinasCore.SQL
{
    public class DbGroupCreator
    {
        public const string PathPrefix = @"C:\Users\EShat\source\repos\AquinasCore\AquinasCore\SQL\";
        public string connectionString = ConfigurationManager.ConnectionStrings["DbReader"].ConnectionString;
        public const string format = "{groupid}";
        public const string roles = "roles.txt";
        public const string tags = "tags.txt";
        public const string tasks = "tasks.txt";
        public const string comments = "comments.txt";
        public const string user_role = "user_role.txt";
        public const string task_users = "task_users.txt";
        public const string task_roles = "task_roles.txt";
        public const string task_tags = "task_tags.txt";
        //
        public const string fill = "fill.txt";
        public const string triggers = "triggers.txt";
        public long id;
        public DbGroupCreator(long id)
        {
            this.id = id;
            CreateRoles();
            CreateTags();
            CreateTasks();
            CreateComments();
            CreateUserRole();
            CreateTaskUsers();
            CreateTaskRoles();
            CreateTaskTags();
            // tables created
            FillTables();
            //CreateTriggers(); - not using rn
        }
        public void Execute(string sql)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
        private void CreateRoles()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + roles)).Replace(format, id.ToString());
            Execute(sql);
        }
        private void CreateTags()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + tags)).Replace(format, id.ToString());
            Execute(sql);
        }
        private void CreateTasks()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + tasks)).Replace(format, id.ToString());
            Execute(sql);
        }
        private void CreateComments()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + comments)).Replace(format, id.ToString());
            Execute(sql);
        }
        private void CreateUserRole()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + user_role)).Replace(format, id.ToString());
            Execute(sql);
        }
        private void CreateTaskUsers()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + task_users)).Replace(format, id.ToString());
            Execute(sql);
        }
        private void CreateTaskRoles()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + task_roles)).Replace(format, id.ToString());
            Execute(sql);
        }
        private void CreateTaskTags()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + task_tags)).Replace(format, id.ToString());
            Execute(sql);
        }
        private void FillTables()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + fill)).Replace(format, id.ToString());
            Execute(sql);
        }
        private void CreateTriggers()
        {
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + triggers)).Replace(format, id.ToString());
            Execute(sql);
        }
    }
    public class DbGroupDeletor
    {
        public const string PathPrefix = @"C:\Users\EShat\source\repos\AquinasCore\AquinasCore\SQL\";
        public string connectionString = ConfigurationManager.ConnectionStrings["DbReader"].ConnectionString;
        public const string delete = "delete.txt";
        public const string format = "{groupid}";
        public long id;
        public DbGroupDeletor(long id)
        {
            this.id = id;
            var sql = string.Join(" ", File.ReadAllLines(PathPrefix + delete)).Replace(format, id.ToString());
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}