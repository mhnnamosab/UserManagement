using UserManagementAPI.Enums;

namespace UserManagementAPI.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
