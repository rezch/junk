using System;
using QRAttendance.Domain.Enums;

namespace QRAttendance.Domain.Entities
{
    public class AttendanceMark
    {
        public Guid Id { get; }
        public DateTime MarkTime { get; }
        public AttendanceStatus Status { get; private set; }
        public AttendanceSource Source { get; private set; }
        public string? CorrectionReason { get; private set; }
        public User Student { get; }
        public Session Session { get; }

        internal AttendanceMark(Guid id, User student, Session session, DateTime markTime,
                                AttendanceStatus status, AttendanceSource source, string? reason = null)
        {
            Id = id;
            Student = student;
            Session = session;
            MarkTime = markTime;
            Status = status;
            Source = source;
            CorrectionReason = reason;
        }

        internal void Correct(AttendanceStatus newStatus, string reason, User actingUser)
        {
            if (actingUser.Role != Role.Teacher && actingUser.Role != Role.Admin)
                throw new UnauthorizedAccessException("Only teacher or admin can correct attendance.");
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Reason must be provided for correction.");

            Status = newStatus;
            CorrectionReason = reason;
            Source = AttendanceSource.ManualCorrection;
        }
    }

    public class StudentAttendanceSummary
    {
        public string StudentName { get; set; }
        public int TotalSessions { get; set; }
        public int Attended { get; set; }
        public int Late { get; set; }
        public int Missed { get; set; }
        public double AttendancePercentage { get; set; }
    }

    public class GroupAttendanceReport
    {
        public string GroupName { get; set; }
        public string CourseName { get; set; }
        public int TotalSessions { get; set; }
        public double AverageAttendancePercentage { get; set; }
        public List<StudentAttendanceSummary> Students { get; set; } = [];
    }
}

