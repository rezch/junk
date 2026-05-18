using System.Collections.Generic;
using System.Linq;
using QRAttendance.Domain.Entities;
using QRAttendance.Domain.Enums;

namespace QRAttendance.Domain.Services
{
    public static class ReportingService
    {
        public static double CalculateGroupAttendancePercentage(IEnumerable<Session> sessions, Group group)
        {
            var relevantSessions = sessions.Where(s => s.Group.Id == group.Id).ToList();
            if (!relevantSessions.Any() || group.Students.Count == 0)
                return 0;

            int totalPossibleMarks = group.Students.Count * relevantSessions.Count;
            int actualMarks = relevantSessions.Sum(s => s.Marks.Count(m => m.Status == AttendanceStatus.Present || m.Status == AttendanceStatus.Late));

            return (double)actualMarks / totalPossibleMarks * 100;
        }
    }
}
