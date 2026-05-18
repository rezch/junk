using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using QRAttendance.Domain.Entities;
using QRAttendance.DataAccess.Interfaces;

namespace QRAttendance.DataAccess.Repositories
{
    public class JsonFileSessionRepository : ISessionRepository
    {
        private readonly string _filePath;
        private List<Session> _sessions;

        public JsonFileSessionRepository(string filePath = "sessions.json")
        {
            _filePath = filePath;
            Load();
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _sessions = JsonSerializer.Deserialize<List<Session>>(json) ?? new List<Session>();
            }
            else
            {
                _sessions = new List<Session>();
            }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_sessions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public Session GetById(Guid id) => _sessions.FirstOrDefault(s => s.Id == id);
        public IEnumerable<Session> GetAll() => _sessions.ToList();
        public void Add(Session session)
        {
            _sessions.Add(session);
            Save();
        }
        public void Update(Session session)
        {
            var existing = GetById(session.Id);
            if (existing != null)
            {
                _sessions.Remove(existing);
                _sessions.Add(session);
                Save();
            }
        }
        public void Delete(Guid id)
        {
            _sessions.RemoveAll(s => s.Id == id);
            Save();
        }
    }
}
