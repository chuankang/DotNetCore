using System.Data;
using System.Data.SqlClient;

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
        private const string DefaultSqlConnectionString = @"Data Source=.;Initial Catalog=OWNDB;User ID=sa;Password=123456;";

        public static IDbConnection GetSqlConnection(string sqlConnectionString = null)
        {
            var config = AppConfigurations.DefaultConnection;

            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                sqlConnectionString = DefaultSqlConnectionString;
            }
            IDbConnection conn = new SqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }

        #endregion
    }
}
