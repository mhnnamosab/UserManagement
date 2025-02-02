using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace UserManagementAPI.Services
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;
		private readonly AppDbContext _context;

		public AuthService(IConfiguration configuration, AppDbContext context)
		{
			_configuration = configuration;
			_context = context;
		}

		public async Task<AuthResponseDto> AuthenticateUserAsync(LoginRequestDto request)
		{
			Console.WriteLine($"Request Admin Hash: {PasswordHasher.HashPassword(request.Password)}");

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
			Console.WriteLine($"Admin Hash: {user.Password}");

			if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.Password))
				return null;

			var token = GenerateJwtToken(user);
			return new AuthResponseDto
			{
				Token = token,
				User = new UserDto { Id = user.Id, Username = user.Username, Role = user.Role }
			};
		}

		private string GenerateJwtToken(User user)
		{
			var jwtSettings = _configuration.GetSection("Jwt");
			var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
			new Claim(ClaimTypes.Name, user.Username),
			new Claim(ClaimTypes.Role, user.Role.ToString()) 
        }),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature
				),
				Issuer = jwtSettings["Issuer"],
				Audience = jwtSettings["Audience"]
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

	}
}
