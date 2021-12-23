using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using AquinasCore.Models.DTO;
namespace AquinasCore.SQL
{
    public static class DbWriter
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DbReader"].ConnectionString;
        public static void Write(string sql)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}