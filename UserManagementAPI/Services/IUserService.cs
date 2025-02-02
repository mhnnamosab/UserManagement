using UserManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagementAPI.Services
{
    public interface IUserService
    {
        Task<object> GetUsersAsync(string search, int page, int pageSize, string sortBy, bool ascending);
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
