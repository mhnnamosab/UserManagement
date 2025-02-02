using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagementAPI.Data;
using UserManagementAPI.Models;
using UserManagementAPI.Enums;
using UserManagementAPI.Utils;
using UserManagementAPI.DTOs;

namespace UserManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IAuditLogService _auditLogService;

        public UserService(AppDbContext context, IAuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        }

        // ✅ Get all users with pagination, search, and sorting
        public async Task<object> GetUsersAsync(string search, int page, int pageSize, string sortBy, bool ascending)
        {
            Console.WriteLine($"Search Query: {search}");

            var query = _context.Users.AsQueryable();

            // Search filter
 
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => EF.Functions.Like(u.Username, $"%{search}%"));
            }

            // Sorting
            query = sortBy.ToLower() switch
            {
                "username" => ascending ? query.OrderBy(u => u.Username) : query.OrderByDescending(u => u.Username),
                "role" => ascending ? query.OrderBy(u => u.Role) : query.OrderByDescending(u => u.Role),
                _ => ascending ? query.OrderBy(u => u.Id) : query.OrderByDescending(u => u.Id)
            };

            var totalCount = await query.CountAsync();

            // Map to DTO to exclude password
            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role
                })
                .ToListAsync();

            return new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = users
            };
        }


        // ✅ Get user by ID
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // ✅ Add new user
        public async Task<User> AddUserAsync(User user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _auditLogService.LogAction("INSERT", "Users", user.Id, "User created.");

            return user;
        }

        // ✅ Update user
        public async Task<User> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return null;

            existingUser.Username = user.Username;
            existingUser.Role = user.Role;
            existingUser.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            await _auditLogService.LogAction("UPDATE", "Users", id, "User updated.");

            return existingUser;
        }

        // ✅ Delete user
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            await _auditLogService.LogAction("DELETE", "Users", id, "User deleted.");

            return true;
        }
    }
}
