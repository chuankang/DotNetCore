using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DotNetCore.Common
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public static class DataBaseConfig
    {
        private static readonly AppConfigurations AppConfigurations;

        static DataBaseConfig()
        {
            AppConfigurations = ServiceLocator.AppConfigurations;
        }

        #region SqlServer链接配置

        /// <summary>
        /// 默认的Sql Server的链接字符串
        /// </summary>
        private const string DefaultSqlConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=OWNDB;User ID=sa;Password=123456;";

        public static IDbConnection GetSqlConnection()
        {
            IDbConnection conn;

            if (AppConfigurations?.UseDbType == "mysql")
            {
                var sqlConnectionString = AppConfigurations?.MySqlConnection;

                if (string.IsNullOrWhiteSpace(sqlConnectionString))
                {
                    sqlConnectionString = DefaultSqlConnectionString;
                }
                conn = new MySqlConnection(sqlConnectionString);
                if (string.IsNullOrEmpty(conn.ConnectionString))
                {
                    conn.Open();
                }
            }
            else
            {
                var sqlConnectionString = AppConfigurations?.DefaultConnection;

                if (string.IsNullOrWhiteSpace(sqlConnectionString))
                {
                    sqlConnectionString = DefaultSqlConnectionString;
                }
                conn = new SqlConnection(sqlConnectionString);
                if (string.IsNullOrEmpty(conn.ConnectionString))
                {
                    conn.Open();
                }
            }
            return conn;
        }

        /// <summary>
        /// sqlserver
        /// </summary>
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(AppConfigurations?.DefaultConnection);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// mysql
        /// </summary>
        public static MySqlConnection GetMySql()
        {
            var conn = new MySqlConnection(AppConfigurations?.MySqlConnection);
            conn.Open();
            return conn;
        }

        #endregion
    }
}
