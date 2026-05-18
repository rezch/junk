using System;
using QRAttendance.Domain.Enums;

namespace QRAttendance.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; private set; }
        public DateTime Timestamp { get; private set; }
        public AuditAction Action { get; private set; }
        public string Details { get; private set; }
        public User User { get; private set; }

        public AuditLog(Guid id, User user, AuditAction action, string details)
        {
            Id = id;
            User = user;
            Action = action;
            Details = details;
            Timestamp = DateTime.UtcNow;
        }
    }
}
