using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;  // Add logging support
using System;
using System.Threading.Tasks;
using UserManagementAPI.DTOs;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;  // Inject ILogger for logging

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for login request.");
                    return BadRequest("Invalid request data.");
                }

                var result = await _authService.AuthenticateUserAsync(request);
                if (result == null)
                {
                    _logger.LogWarning("Failed login attempt for user: {Username}", request.Username);  // Log failed login attempts
                    return Unauthorized("Invalid username or password.");
                }

                _logger.LogInformation("User {Username} successfully logged in.", request.Username);  // Log successful login
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to authenticate user: {Username}.", request.Username);  // Log the error with exception details
                return StatusCode(500, "An unexpected error occurred while processing the login request.");
            }
        }
    }
}
