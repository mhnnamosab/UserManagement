namespace UserManagementAPI.Models
{
    public class AuditLog
    {
        public int Id { get; set; }              // Unique identifier for each log entry
        public string Action { get; set; }       // Type of action (Insert, Update, Delete)
        public string TableName { get; set; }    // Name of the table being modified
        public int RecordId { get; set; }        // ID of the record being modified
        public DateTime Timestamp { get; set; }  // When the action occurred
        public string Details { get; set; }      // Additional details about the action
    }
}