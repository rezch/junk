using System;
using System.Collections.Generic;
using System.Linq;
using QRAttendance.Domain.Entities;
using QRAttendance.Domain.Services;
using QRAttendance.Domain.Enums;
using QRAttendance.DataAccess.Interfaces;

namespace QRAttendance.Application
{
    public class AttendanceService(
        ISessionRepository sessionRepo,
        IUserRepository userRepo,
        ICourseRepository courseRepo,
        IGroupRepository groupRepo)
    {
        private readonly ISessionRepository _sessionRepository = sessionRepo;
        private readonly IUserRepository _userRepository = userRepo;
        private readonly ICourseRepository _courseRepository = courseRepo;
        private readonly IGroupRepository _groupRepository = groupRepo;
        private readonly AttendanceReportingFacade _attendanceReporting =
            new (sessionRepo, userRepo, groupRepo);

        public Session CreateSession(Guid courseId, Guid groupId, Guid lecturerId, 
                                      DateTime date, TimeSpan startTime, int lateThreshold = 15)
        {
            var course = _courseRepository.GetById(courseId);
            var group = _groupRepository.GetById(groupId);
            var lecturer = _userRepository.GetById(lecturerId);

            if (course == null) throw new ArgumentException("Курс не найден");
            if (group == null) throw new ArgumentException("Группа не найдена");
            if (lecturer == null) throw new ArgumentException("Преподаватель не найден");
            if (lecturer.Role != Role.Teacher && lecturer.Role != Role.Admin)
                throw new InvalidOperationException("Только преподаватель или админ может создать занятие");

            var session = new Session(Guid.NewGuid(), course, group, lecturer, date, startTime, lateThreshold);
            _sessionRepository.Add(session);
            return session;
        }

        public AttendanceMark MarkAttendance(Guid sessionId, Guid studentId, string qrToken, 
                                             DateTime currentTime, string signingKey,
                                             Func<string, string, bool> validateSignature)
        {
            var session = _sessionRepository.GetById(sessionId)
                ?? throw new ArgumentException("Занятие не найдено");
            var student = _userRepository.GetById(studentId)
                ?? throw new ArgumentException("Студент не найден");
            var qrCode = new QRCode(qrToken, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(60));

            var mark = session.MarkAttendance(student, qrCode, currentTime, signingKey, validateSignature);
            _sessionRepository.Update(session);
            return mark;
        }

        public void CorrectAttendance(Guid sessionId, Guid studentId, AttendanceStatus newStatus, 
                                      string reason, Guid actingUserId)
        {
            var session = _sessionRepository.GetById(sessionId);
            if (session == null) throw new ArgumentException("Занятие не найдено");

            var actingUser = _userRepository.GetById(actingUserId);
            if (actingUser == null) throw new ArgumentException("Пользователь не найден");

            var student = _userRepository.GetById(studentId);
            if (student == null) throw new ArgumentException("Студент не найден");

            session.CorrectAttendance(student, newStatus, reason, actingUser);
            _sessionRepository.Update(session);
        }

        public double GetGroupAttendancePercentage(Guid groupId, DateTime from, DateTime to)
        {
            var group = _groupRepository.GetById(groupId);
            if (group == null) throw new ArgumentException("Группа не найдена");

            var sessions = _sessionRepository.GetAll()
                .Where(s => s.Group.Id == groupId && s.Date >= from && s.Date <= to)
                .ToList();

            return ReportingService.CalculateGroupAttendancePercentage(sessions, group);
        }

        public IEnumerable<Session> GetAllSessions() => _sessionRepository.GetAll();
        public IEnumerable<Course> GetAllCourses() => _courseRepository.GetAll();
        public IEnumerable<Group> GetAllGroups() => _groupRepository.GetAll();
        public IEnumerable<User> GetAllTeachers() => _userRepository.GetAll().Where(u => u.Role == Role.Teacher);
        public IEnumerable<User> GetAllStudents() => _userRepository.GetAll().Where(u => u.Role == Role.Student);
    }
}


