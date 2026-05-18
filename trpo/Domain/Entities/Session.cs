using System;
using System.Collections.Generic;
using System.Linq;
using QRAttendance.Domain.Enums;

namespace QRAttendance.Domain.Entities
{
    public class Session
    {
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public SessionStatus Status { get; private set; }
        public int LateThresholdMinutes { get; private set; }
        public Course Course { get; private set; }
        public Group Group { get; private set; }
        public User Lecturer { get; private set; }

        private readonly List<AttendanceMark> _marks = new List<AttendanceMark>();
        public IReadOnlyList<AttendanceMark> Marks => _marks.AsReadOnly();

        public Session(Guid id, Course course, Group group, User lecturer, 
                       DateTime date, TimeSpan startTime, int lateThresholdMinutes = 15)
        {
            if (lecturer.Role != Role.Teacher && lecturer.Role != Role.Admin)
                throw new ArgumentException("Only teacher or admin can create a session.");

            Id = id;
            Course = course;
            Group = group;
            Lecturer = lecturer;
            Date = date;
            StartTime = startTime;
            LateThresholdMinutes = lateThresholdMinutes;
            Status = SessionStatus.Active;
        }

        public void Close(User actingUser)
        {
            if (actingUser.Id != Lecturer.Id && actingUser.Role != Role.Admin)
                throw new UnauthorizedAccessException("Only the lecturer or admin can close the session.");
            if (Status != SessionStatus.Active)
                throw new InvalidOperationException("Session is already closed.");
            Status = SessionStatus.Closed;
        }

        public QRCode GenerateQRCode(TimeSpan ttl, string signingKey)
        {
            if (Status != SessionStatus.Active)
                throw new InvalidOperationException("Cannot generate QR code for a closed session.");

            var issuedAt = DateTime.UtcNow;
            var expiresAt = issuedAt.Add(ttl);
            var payload = $"{Id}|{issuedAt:O}|{expiresAt:O}|{Lecturer.Id}";
            var token = $"{payload}|{HashHelper.ComputeHash(payload + signingKey)}";
            return new QRCode(token, issuedAt, expiresAt);
        }

        public AttendanceMark MarkAttendance(User student, QRCode qrCode, DateTime currentTime, 
                                             string signingKey, Func<string, string, bool> validateSignature)
        {
            if (student.Role != Role.Student)
                throw new InvalidOperationException("Only students can mark attendance.");
            if (!student.IsActive)
                throw new InvalidOperationException("Student account is inactive.");
            if (Status != SessionStatus.Active)
                throw new InvalidOperationException("Attendance cannot be marked for a closed session.");
            if (!qrCode.IsValid(currentTime))
                throw new InvalidOperationException("QR code has expired.");

            if (!validateSignature(qrCode.Token, signingKey))
                throw new InvalidOperationException("Invalid QR code signature.");

            if (!Group.ContainsStudent(student))
                throw new InvalidOperationException("Student is not enrolled in this group.");
            if (_marks.Any(m => m.Student.Id == student.Id))
                throw new InvalidOperationException("Student has already marked attendance for this session.");

            var sessionStartDateTime = Date.Add(StartTime);
            var minutesLate = (currentTime - sessionStartDateTime).TotalMinutes;
            var status = minutesLate > LateThresholdMinutes ? AttendanceStatus.Late : AttendanceStatus.Present;

            var mark = new AttendanceMark(Guid.NewGuid(), student, this, currentTime, 
                                          status, AttendanceSource.QR);
            _marks.Add(mark);
            return mark;
        }

        public void CorrectAttendance(User student, AttendanceStatus newStatus, string reason, User actingUser)
        {
            if (actingUser.Role != Role.Teacher && actingUser.Role != Role.Admin)
                throw new UnauthorizedAccessException("Only teacher or admin can correct attendance.");
            if (actingUser.Id != Lecturer.Id && actingUser.Role != Role.Admin)
                throw new UnauthorizedAccessException("Only the lecturer or admin can correct attendance for this session.");

            var mark = _marks.FirstOrDefault(m => m.Student.Id == student.Id);
            if (mark == null)
                throw new InvalidOperationException("No attendance mark found for this student.");

            mark.Correct(newStatus, reason, actingUser);
        }

        public double GetAttendancePercentage()
        {
            if (Group.Students.Count == 0) return 0;
            var presentCount = _marks.Count(m => m.Status == AttendanceStatus.Present || m.Status == AttendanceStatus.Late);
            return (double)presentCount / Group.Students.Count * 100;
        }
    }
}
