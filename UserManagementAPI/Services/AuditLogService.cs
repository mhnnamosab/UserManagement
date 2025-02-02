using UserManagementAPI.Data;
using UserManagementAPI.Models;
using System.Threading.Tasks;


namespace UserManagementAPI.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _context;

        public AuditLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAction(string action, string tableName, int recordId, string details)
        {
            var log = new AuditLog
            {
                Action = action,
                TableName = tableName,
                RecordId = recordId,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
