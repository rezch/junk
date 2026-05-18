using System;

namespace QRAttendance.Domain.Enums
{
    public enum Role
    {
        Student,
        Teacher,
        Directorate,
        Admin
    }

    public enum SessionStatus
    {
        Active,
        Closed
    }

    public enum AttendanceStatus
    {
        Present,
        Late,
        Absent,
        Cancelled
    }

    public enum AttendanceSource
    {
        QR,
        ManualCorrection
    }

    public enum AuditAction
    {
        Login,
        CreateSession,
        CloseSession,
        MarkAttendance,
        CorrectAttendance,
        ExportReport
    }
}
