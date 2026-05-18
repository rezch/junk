using System;
using System.Collections.Generic;
using System.Linq;
using QRAttendance.Domain.Entities;
using QRAttendance.DataAccess.Interfaces;

namespace QRAttendance.DataAccess.Repositories
{
    public class InMemoryGroupRepository : IGroupRepository
    {
        private readonly List<Group> _items = new List<Group>();

        public Group GetById(Guid id) => _items.FirstOrDefault(i => i.Id == id);
        public IEnumerable<Group> GetAll() => _items.ToList();
        public void Add(Group item) => _items.Add(item);
        public void Update(Group item)
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
