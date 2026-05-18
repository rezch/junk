using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using QRAttendance.Domain.Entities;
using QRAttendance.DataAccess.Interfaces;

namespace QRAttendance.DataAccess.Repositories
{
    public class JsonFileCourseRepository : ICourseRepository
    {
        private readonly string _filePath;
        private List<Course> _courses;

        public JsonFileCourseRepository(string filePath = "courses.json")
        {
            _filePath = filePath;
            Load();
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _courses = JsonSerializer.Deserialize<List<Course>>(json) ?? new List<Course>();
            }
            else
            {
                _courses = new List<Course>();
            }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_courses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public Course GetById(Guid id) => _courses.FirstOrDefault(c => c.Id == id);
        public IEnumerable<Course> GetAll() => _courses.ToList();
        public void Add(Course course)
        {
            _courses.Add(course);
            Save();
        }
        public void Update(Course course)
        {
            var existing = GetById(course.Id);
            if (existing != null)
            {
                _courses.Remove(existing);
                _courses.Add(course);
                Save();
            }
        }
        public void Delete(Guid id)
        {
            _courses.RemoveAll(c => c.Id == id);
            Save();
        }
    }
}
