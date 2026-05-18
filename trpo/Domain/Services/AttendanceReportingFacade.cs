using QRAttendance.DataAccess.Interfaces;
using QRAttendance.Domain.Entities;
using QRAttendance.Domain.Enums;

namespace QRAttendance.Domain.Services
{
    public class AttendanceReportingFacade(
            ISessionRepository sessionRepository,
            IUserRepository userRepository,
            IGroupRepository groupRepository)
    {
        private readonly ISessionRepository _sessionRepository = sessionRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IGroupRepository _groupRepository = groupRepository;

        public StudentAttendanceSummary GetStudentGlobalSummary(Guid studentId)
        {
            var student = _userRepository.GetById(studentId)
                ?? throw new ArgumentException("Студент не найден");

            var relevantSessions = _sessionRepository.GetAll()
                .Where(s => s.Group.ContainsStudent(student))
                .ToList();

            return CalculateStudentStats(student, relevantSessions);
        }

        public GroupAttendanceReport GetGroupReport(Guid groupId, Guid courseId)
        {
            var group = _groupRepository.GetById(groupId)
                ?? throw new ArgumentException("Группа не найдена");

            var relevantSessions = _sessionRepository.GetAll()
                .Where(s => s.Group.Id == groupId && s.Course.Id == courseId)
                .ToList();

            var report = new GroupAttendanceReport
            {
                GroupName = group.Name,
                CourseName = relevantSessions.FirstOrDefault()?.Course.Name ?? "Неизвестный курс",
                TotalSessions = relevantSessions.Count
            };

            if (relevantSessions.Count == 0 || group.Students.Count == 0)
                return report;

            foreach (var student in group.Students)
            {
                var studentStats = CalculateStudentStats(student, relevantSessions);
                report.Students.Add(studentStats);
            }

            report.AverageAttendancePercentage = Math.Round(
                    report.Students.Average(s => s.AttendancePercentage), 2);
            report.Students = [.. report.Students.OrderBy(s => s.StudentName)];

            return report;
        }

        private static StudentAttendanceSummary CalculateStudentStats(
                User student, List<Session> sessions)
        {
            int attended = 0;
            int late = 0;

            foreach (var session in sessions)
            {
                var mark = session.Marks.FirstOrDefault(m => m.Student.Id == student.Id);
                if (mark == null) continue;
                if (mark.Status == AttendanceStatus.Present) attended++;
                if (mark.Status == AttendanceStatus.Late) late++;
            }

            int presentEquivalent = attended + late;
            int total = sessions.Count;

            double percentage = total > 0
                ? (double)presentEquivalent / total * 100
                : 0;

            return new StudentAttendanceSummary
            {
                StudentName = student.Name,
                TotalSessions = total,
                Attended = attended,
                Late = late,
                Missed = total - presentEquivalent,
                AttendancePercentage = Math.Round(percentage, 2)
            };
        }
    }
}