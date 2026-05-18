using System;
using System.Collections.Generic;
using System.Linq;
using QRAttendance.Domain.Entities;
using QRAttendance.DataAccess.Interfaces;

namespace QRAttendance.DataAccess.Repositories
{
    public class InMemorySessionRepository : ISessionRepository
    {
        private readonly List<Session> _items = new List<Session>();

        public Session GetById(Guid id) => _items.FirstOrDefault(i => i.Id == id);
        public IEnumerable<Session> GetAll() => _items.ToList();
        public void Add(Session item) => _items.Add(item);
        public void Update(Session item)
        {
            var existing = GetById(item.Id);
            if (existing != null)
            {
                _items.Remove(existing);
                _items.Add(item);
            }
        }
        public void Delete(Guid id) => _items.RemoveAll(i => i.Id == id);
    }
}
