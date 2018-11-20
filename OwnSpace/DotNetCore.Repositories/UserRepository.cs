using Dapper;
using DotNetCore.Common;
using DotNetCore.Interface;
using DotNetCore.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Common.Utils;
using DotNetCore.Models.Team;

namespace DotNetCore.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<List<User>> GetUserListAsync()
        {
            using (var conn = DataBaseConfig.GetSqlConnection())
            {
                const string sql = @"SELECT Name AS UserName, 
                                            Birthday 
                                     FROM dbo.BasicInfomation";

                var usetList = await conn.QueryAsync<User>(sql);

                return usetList.ToList();
            }
        }

        public string GetAddressByName(string name)
        {
            const string sql = @"SELECT Address
                                 FROM dbo.BasicInfomation 
                                 WHERE Name = @name";

            return SqlHelper.Query<string>(sql, new { name }).FirstOrDefault();
        }

        public int InsertTeam(List<Team> teams)
        {
            var teamTable = EntityMapUtil.FromObject(teams);
            var bulkDic = new Dictionary<string, DataTable>
            {
                { TableContant.Team, teamTable }
            };

            var resultMsg = BulkUtil.Execute(bulkDic);

            return resultMsg.MsgState ? 1 : 0;
        }
    }
}
