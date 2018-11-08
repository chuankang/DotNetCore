using DotNetCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCore.Interface
{
    public interface IUserService
    {
        Task<List<User>> GetUserListAsync();
        string GetAddressByName(string name);
    }
}
