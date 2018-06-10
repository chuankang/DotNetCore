using Dapper;
using DotNetCore.Common;
using DotNetCore.Interface;
using DotNetCore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<List<User>> GetUserListAsync()
        {
            using (var conn = DataBaseConfig.GetSqlConnection())
            {
                const string insertSql = @"SELECT UserName, Birthday FROM dbo.[User]";

                var usetList = await conn.QueryAsync<User>(insertSql);

                return usetList.ToList();
            }
        }
    }
}
