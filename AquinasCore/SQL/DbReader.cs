using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
namespace AquinasCore.SQL
{
    public static class DbReader
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DbReader"].ConnectionString;
        public static DataTable Read(string sql)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
        }
    }
}