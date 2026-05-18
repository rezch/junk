using System;
using System.Collections.Generic;
using System.Linq;
using QRAttendance.Domain.Entities;
using QRAttendance.DataAccess.Interfaces;

namespace QRAttendance.DataAccess.Repositories
{
    public class InMemoryCourseRepository : ICourseRepository
    {
        private readonly List<Course> _items = new List<Course>();

        public Course GetById(Guid id) => _items.FirstOrDefault(i => i.Id == id);
        public IEnumerable<Course> GetAll() => _items.ToList();
        public void Add(Course item) => _items.Add(item);
        public void Update(Course item)
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
