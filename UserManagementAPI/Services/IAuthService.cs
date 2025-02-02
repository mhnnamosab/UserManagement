using UserManagementAPI.DTOs;
using System.Threading.Tasks;

namespace UserManagementAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> AuthenticateUserAsync(LoginRequestDto request);
    }
}
