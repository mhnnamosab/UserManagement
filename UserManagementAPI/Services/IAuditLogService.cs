namespace UserManagementAPI.Services
{
    public interface IAuditLogService
    {
        Task LogAction(string action, string tableName, int recordId, string details);
    }
}
