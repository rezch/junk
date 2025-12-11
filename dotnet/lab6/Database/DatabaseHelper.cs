using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using lab5.Models;

namespace lab5.Database
{
    public class DatabaseHelper
    {
        private readonly string _connectionString = "Data Source=students.db";

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
            var students = new List<Student>();
            EnsureCreated();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Age FROM Students";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2)
                    });
                }
            }
            return students;
        }

        public bool AddStudent(Student student)
        {
            try
            {
                EnsureCreated();
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = "INSERT INTO Students (Id, Name, Age) VALUES (@id, @name, @age)";
                command.Parameters.AddWithValue("@id", student.Id);
                command.Parameters.AddWithValue("@name", student.Name);
                command.Parameters.AddWithValue("@age", student.Age);
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                // Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}