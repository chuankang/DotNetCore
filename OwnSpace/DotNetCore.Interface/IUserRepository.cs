using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.Models;

namespace DotNetCore.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetUserListAsync();

        string GetAddressByName(string name);
    }
}
