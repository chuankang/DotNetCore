using DotNetCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.Models.Team;

namespace DotNetCore.Interface
{
    public interface IUserService
    {
        Task<List<User>> GetUserListAsync();
        string GetAddressByName(string name);
        int InsertTeam(List<Team> teams);
    }
}
