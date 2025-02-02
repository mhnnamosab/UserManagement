using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync(string search, int page, int pageSize, string sortBy, bool ascending);
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
