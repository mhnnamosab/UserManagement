using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Models;
using UserManagementAPI.Enums;
using UserManagementAPI.Utils;
using BCrypt.Net;

namespace UserManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Precomputed password hashes (run once and copy the output)
            string adminHash = "$2a$11$QSGeo.5/DVpy5J9oGtsVqOMaHXgQKhbNZIPSDc6fEvlL9mHxlKLz6";  // Static hash
            string userHash = "$2a$12$sdg8FsAD7gZd8sdfB20dG5sdf3dQEZkKc1pHbT/Kfw3pdL9uBdsXm";  // Static hash
            // Seed roles
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = adminHash,
                    Role = UserRole.Admin,
                    CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), // Static UTC time
                    UpdatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)  // Same as CreatedAt
                },
                new User
                {
                    Id = 2,
                    Username = "user",
                    Password = userHash,
                    Role = UserRole.User,
                    CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), // Static UTC time
                    UpdatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)  // Same as CreatedAt
                }
            );
        }
    }
}