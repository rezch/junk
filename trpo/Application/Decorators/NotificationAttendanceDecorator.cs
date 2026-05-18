using QRAttendance.Application.Interfaces;
using QRAttendance.DataAccess.Interfaces;
using QRAttendance.Domain.Entities;
using QRAttendance.Domain.Enums;

namespace QRAttendance.Application.Decorators
{
    public class NotificationAttendanceDecorator(
        IAttendanceService innerService,
        INotificationService notificationService,
        IUserRepository userRepository) : IAttendanceService
    {
        private readonly IAttendanceService _innerService = innerService;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IUserRepository _userRepository = userRepository;

        public AttendanceMark MarkAttendance(
                Guid sessionId, Guid studentId, string qrToken, DateTime currentTime, string signingKey)
        {
            return _innerService.MarkAttendance(sessionId, studentId, qrToken, currentTime, signingKey);
        }

        public void CorrectAttendance(
                Guid sessionId, Guid studentId, AttendanceStatus newStatus, string reason, Guid actingUserId)
        {
            _innerService.CorrectAttendance(sessionId, studentId, newStatus, reason, actingUserId);

            var student = _userRepository.GetById(studentId);

            if (student != null)
            {
                const string subject = "Внимание: Изменение статуса посещаемости";
                string body = $"Здравствуйте, {student.Name}!\n\n" +
                              $"Преподаватель изменил ваш статус на занятии на: {newStatus}.\n" +
                              $"Указанная причина: {reason}\n\n" +
                              "Если вы не согласны, пожалуйста, свяжитесь с деканатом.";

                _notificationService.SendMessage(student.Email, subject, body);
            }
        }

        public Session CreateSession(
                Guid courseId, Guid groupId, Guid lecturerId, DateTime date, TimeSpan startTime, int lateThreshold)
        {
            return _innerService.CreateSession(courseId, groupId, lecturerId, date, startTime, lateThreshold);
        }

        AttendanceMark IAttendanceService.MarkAttendance(
                Guid sessionId, Guid studentId, string qrToken, DateTime currentTime, string signingKey)
        {
            return _innerService.MarkAttendance(sessionId, studentId, qrToken, currentTime, signingKey);
        }

        public double GetGroupAttendancePercentage(Guid groupId, DateTime from, DateTime to)
        {
            return _innerService.GetGroupAttendancePercentage(groupId, from, to);
        }
    }
}