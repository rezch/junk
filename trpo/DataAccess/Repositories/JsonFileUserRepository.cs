using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using QRAttendance.Domain.Entities;
using QRAttendance.DataAccess.Interfaces;

namespace QRAttendance.DataAccess.Repositories
{
    public class JsonFileUserRepository : IUserRepository
    {
        private readonly string _filePath;
        private List<User> _users;

        public JsonFileUserRepository(string filePath = "users.json")
        {
            _filePath = filePath;
            Load();
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                _users = new List<User>();
            }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public User GetById(Guid id) => _users.FirstOrDefault(u => u.Id == id);
        public IEnumerable<User> GetAll() => _users.ToList();
        public void Add(User user)
        {
            _users.Add(user);
            Save();
        }
        public void Update(User user)
        {
            var existing = GetById(user.Id);
            if (existing != null)
            {
                _users.Remove(existing);
                _users.Add(user);
                Save();
            }
        }
        public void Delete(Guid id)
        {
            _users.RemoveAll(u => u.Id == id);
            Save();
        }
    }
}
