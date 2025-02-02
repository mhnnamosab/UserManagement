
namespace UserManagementAPI.Utils
{
    using BCrypt = BCrypt.Net.BCrypt;

    public static class PasswordHasher
    {
        // Hash a password
        public static string HashPassword(string password)
        {

            return BCrypt.HashPassword(password);
        }

        // Verify a password
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Verify(password, hashedPassword);
        }
    }
}