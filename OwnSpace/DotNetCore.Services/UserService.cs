using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.Interface;
using DotNetCore.Models;

namespace DotNetCore.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetUserListAsync()
        {
            return await _userRepository.GetUserListAsync();
        }

        public string GetAddressByName(string name)
        {
            return _userRepository.GetAddressByName(name);
        }
    }
}
