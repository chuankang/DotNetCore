using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace DotNetCore.Common
{
    public static class SqlHelper
    {
        public static List<T> Query<T>(string query, object param = null)
        {
            using (var conn = DataBaseConfig.GetSqlConnection())
            {
                return conn.Query<T>(query, param)?.ToList();
            }
        }

        public static int Execute(string query, object param = null)
        {
            using (var conn = DataBaseConfig.GetSqlConnection())
            {
                return conn.Execute(query, param);
            }
        }
    }
}
