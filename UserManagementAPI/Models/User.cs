using UserManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid role.")]
        public UserRole Role { get; set; } // Use the enum here

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}