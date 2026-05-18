using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using QRAttendance.Domain.Entities;
using QRAttendance.DataAccess.Interfaces;

namespace QRAttendance.DataAccess.Repositories
{
    public class JsonFileGroupRepository : IGroupRepository
    {
        private readonly string _filePath;
        private List<Group> _groups;

        public JsonFileGroupRepository(string filePath = "groups.json")
        {
            _filePath = filePath;
            Load();
        }

        private void Load()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _groups = JsonSerializer.Deserialize<List<Group>>(json) ?? new List<Group>();
            }
            else
            {
                _groups = new List<Group>();
            }
        }

        private void Save()
        {
            var json = JsonSerializer.Serialize(_groups, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public Group GetById(Guid id) => _groups.FirstOrDefault(g => g.Id == id);
        public IEnumerable<Group> GetAll() => _groups.ToList();
        public void Add(Group group)
        {
            _groups.Add(group);
            Save();
        }
        public void Update(Group group)
        {
            var existing = GetById(group.Id);
            if (existing != null)
            {
                _groups.Remove(existing);
                _groups.Add(group);
                Save();
            }
        }
        public void Delete(Guid id)
        {
            _groups.RemoveAll(g => g.Id == id);
            Save();
        }
    }
}
