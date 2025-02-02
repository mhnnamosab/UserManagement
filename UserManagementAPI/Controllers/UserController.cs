using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementAPI.Models;
using UserManagementAPI.Services;
using UserManagementAPI.Enums;
using Microsoft.Extensions.Logging;

namespace UserManagementAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // Get users with search, pagination, and sorting
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            string search = "",
            int page = 1,
            int pageSize = 10,
            string sortBy = "Id",
            bool ascending = true)
        {
            try
            {
                var result = await _userService.GetUsersAsync(search, page, pageSize, sortBy, ascending);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");  // Log the error
                return StatusCode(500, "An unexpected error occurred while retrieving users.");
            }
        }

        // Get a user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return NotFound("User not found.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user by ID.");  // Log the error
                return StatusCode(500, "An unexpected error occurred while retrieving the user.");
            }
        }

        // Admin-only: Add a new user
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdUser = await _userService.AddUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user.");  // Log the error
                return StatusCode(500, "An unexpected error occurred while adding the user.");
            }
        }

        // Admin-only: Update a user
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedUser = await _userService.UpdateUserAsync(id, user);
                if (updatedUser == null) return NotFound("User not found.");

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the user with ID: {id}.");  // Log the error
                return StatusCode(500, "An unexpected error occurred while updating the user.");
            }
        }

        // Admin-only: Delete a user
        [Authorize(Roles = nameof(UserRole.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var success = await _userService.DeleteUserAsync(id);
                if (!success) return NotFound("User not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the user with ID: {id}.");  // Log the error
                return StatusCode(500, "An unexpected error occurred while deleting the user.");
            }
        }
    }
}
