using QRAttendance.Domain.Entities;
using QRAttendance.Domain.Enums;

namespace QRAttendance.Application.Interfaces
{
    public interface IAttendanceService
    {
        public Session CreateSession(
                Guid courseId, Guid groupId, Guid lecturerId, DateTime date, TimeSpan startTime, int lateThreshold);
        AttendanceMark MarkAttendance(
                Guid sessionId, Guid studentId, string qrToken, DateTime currentTime, string signingKey);
        void CorrectAttendance(
                Guid sessionId, Guid studentId, AttendanceStatus newStatus, string reason, Guid actingUserId);
        public double GetGroupAttendancePercentage(
                Guid groupId, DateTime from, DateTime to);
    }

    public interface INotificationService
    {
        void SendMessage(string toEmail, string subject, string body);
    }
}