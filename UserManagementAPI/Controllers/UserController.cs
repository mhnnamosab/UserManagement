using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementAPI.Models;
using UserManagementAPI.Services;
using UserManagementAPI.Enums;

namespace UserManagementAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(
            string search = "",
            int page = 1,
            int pageSize = 10,
            string sortBy = "Id",
            bool ascending = true)
        {
            var result = await _userService.GetUsersAsync(search, page, pageSize, sortBy, ascending);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        // ✅ Admin-only: Add a new user
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        // ✅ Admin-only: Update user
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedUser = await _userService.UpdateUserAsync(id, user);
            if (updatedUser == null) return NotFound("User not found.");

            return Ok(updatedUser);
        }

        // ✅ Admin-only: Delete user
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound("User not found.");

            return NoContent();
        }
    }
}
