using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using lab5.Models;

namespace lab5.Database
{
    public class DatabaseHelper(SchoolContext context)
    {
        private readonly SchoolContext _context = context;

        static private void EnsureCreated()
        {
            using var context = new SchoolContext();
            context.Database.EnsureCreated();
            if (!context.Students.Any())
            {
                context.Students.Add(new Student { Name = "Иван", Age = 22 });
                context.Students.Add(new Student { Name = "Мария", Age = 20 });
                context.SaveChanges();
            }
        }

        public List<Student> GetStudents()
        {
            return [.. _context.Students];
        }

        public bool AddStudent(Student student)
        {
            _context.Students.Add(student);
            return _context.SaveChanges() > 0;
        }
    }
}