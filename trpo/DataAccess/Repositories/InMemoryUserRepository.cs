using System;
using System.Collections.Generic;
using System.Linq;
using QRAttendance.Domain.Entities;
using QRAttendance.DataAccess.Interfaces;

namespace QRAttendance.DataAccess.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _items = new List<User>();

        public User GetById(Guid id) => _items.FirstOrDefault(i => i.Id == id);
        public IEnumerable<User> GetAll() => _items.ToList();
        public void Add(User item) => _items.Add(item);
        public void Update(User item)
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
