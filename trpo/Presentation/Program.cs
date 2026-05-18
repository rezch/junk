using System;
using System.Collections.Generic;
using System.Linq;
using QRAttendance.Application;
using QRAttendance.Domain.Entities;
using QRAttendance.Domain.Enums;
using QRAttendance.DataAccess.Interfaces;
using QRAttendance.DataAccess.Repositories;

namespace QRAttendance.Presentation
{
    class Program
    {
        private static AttendanceService? _attendanceService;
        private static readonly string _signingKey = "super_secret_key_123";

        static void Main(string[] args)
        {
            var sessionRepo = new JsonFileSessionRepository();
            var userRepo = new JsonFileUserRepository();
            var courseRepo = new JsonFileCourseRepository();
            var groupRepo = new JsonFileGroupRepository();

            _attendanceService = new AttendanceService(sessionRepo, userRepo, courseRepo, groupRepo);
            SeedData(userRepo, courseRepo, groupRepo);

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Система учёта посещаемости ===");
                Console.WriteLine("1. Создать занятие");
                Console.WriteLine("2. Показать все занятия");
                Console.WriteLine("3. Отметить посещаемость (QR)");
                Console.WriteLine("4. Ручная корректировка");
                Console.WriteLine("5. Отчёт по группе за период");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": CreateSession(); break;
                    case "2": ShowSessions(); break;
                    case "3": MarkAttendance(); break;
                    case "4": CorrectAttendance(); break;
                    case "5": ShowReport(); break;
                    case "0": exit = true; break;
                    default: Console.WriteLine("Неверный ввод"); break;
                }
                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void SeedData(IUserRepository userRepo, ICourseRepository courseRepo, IGroupRepository groupRepo)
        {
            var teacher = new User(Guid.NewGuid(), "Иван Петров", "ivan@university.ru", Role.Teacher);
            userRepo.Add(teacher);

            var course = new Course(Guid.NewGuid(), "Программирование", "CS101");
            courseRepo.Add(course);

            var group = new Group(Guid.NewGuid(), "БИСТ-23-ПО-2", 2023);
            var student = new User(Guid.NewGuid(), "Алексей Смирнов", "alex@student.ru", Role.Student);
            group.AddStudent(student);
            userRepo.Add(student);
            groupRepo.Add(group);
        }

        static void CreateSession()
        {
            var courses = _attendanceService!.GetAllCourses().ToList();
            var groups = _attendanceService!.GetAllGroups().ToList();
            var teachers = _attendanceService!.GetAllTeachers().ToList();

            if (!courses.Any() || !groups.Any() || !teachers.Any())
            {
                Console.WriteLine("Не хватает данных для создания занятия");
                return;
            }

            var course = courses.First();
            var group = groups.First();
            var teacher = teachers.First();

            Console.Write("Введите дату занятия (гггг-мм-дд): ");
            var dateStr = Console.ReadLine();
            Console.Write("Введите время начала (чч:мм): ");
            var timeStr = Console.ReadLine();

            if (DateTime.TryParse(dateStr, out var date) && TimeSpan.TryParse(timeStr, out var time))
            {
                try
                {
                    var session = _attendanceService!.CreateSession(course.Id, group.Id, teacher.Id, date, time);
                    Console.WriteLine($"Занятие создано. ID: {session.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Некорректный формат даты или времени");
            }
        }

        static void ShowSessions()
        {
            var sessions = _attendanceService!.GetAllSessions();
            if (!sessions.Any())
            {
                Console.WriteLine("Нет занятий.");
                return;
            }
            foreach (var s in sessions)
            {
                Console.WriteLine($"ID: {s.Id}, Курс: {s.Course.Name}, Группа: {s.Group.Name}, Дата: {s.Date:yyyy-MM-dd}, Время: {s.StartTime:hh\\:mm}, Статус: {s.Status}");
            }
        }

        static void MarkAttendance()
        {
            Console.Write("Введите ID занятия: ");
            if (!Guid.TryParse(Console.ReadLine(), out var sessionId))
            {
                Console.WriteLine("Неверный ID");
                return;
            }
            Console.Write("Введите ID студента: ");
            if (!Guid.TryParse(Console.ReadLine(), out var studentId))
            {
                Console.WriteLine("Неверный ID");
                return;
            }
            Console.Write("Введите QR-токен: ");
            var qrToken = Console.ReadLine();

            bool validateSignature(string token, string key) => true;

            try
            {
                var mark = _attendanceService!.MarkAttendance(sessionId, studentId, qrToken, DateTime.Now, _signingKey, validateSignature);
                Console.WriteLine($"Отметка успешна. Статус: {mark.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void CorrectAttendance()
        {
            Console.Write("Введите ID занятия: ");
            if (!Guid.TryParse(Console.ReadLine(), out var sessionId)) return;
            Console.Write("Введите ID студента: ");
            if (!Guid.TryParse(Console.ReadLine(), out var studentId)) return;
            Console.Write("Введите новый статус (Present, Late, Absent, Cancelled): ");
            var statusStr = Console.ReadLine();
            if (!Enum.TryParse<AttendanceStatus>(statusStr, true, out var newStatus))
            {
                Console.WriteLine("Неверный статус");
                return;
            }
            Console.Write("Причина корректировки: ");
            var reason = Console.ReadLine();
            Console.Write("Ваш ID (пользователь, выполняющий корректировку): ");
            if (!Guid.TryParse(Console.ReadLine(), out var actingUserId)) return;

            try
            {
                _attendanceService!.CorrectAttendance(sessionId, studentId, newStatus, reason, actingUserId);
                Console.WriteLine("Корректировка выполнена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ShowReport()
        {
            Console.Write("Введите ID группы: ");
            if (!Guid.TryParse(Console.ReadLine(), out var groupId)) return;
            Console.Write("Дата начала (гггг-мм-дд): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var from)) return;
            Console.Write("Дата окончания (гггг-мм-дд): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var to)) return;

            try
            {
                var percentage = _attendanceService!.GetGroupAttendancePercentage(groupId, from, to);
                Console.WriteLine($"Процент посещаемости группы за период: {percentage:F2}%");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}






