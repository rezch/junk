using System;
using QRAttendance.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace QRAttendance.Domain.Entities
{
    public class Group
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int YearOfEntry { get; private set; }
        private readonly List<User> _students = new List<User>();
        public IReadOnlyList<User> Students => _students.AsReadOnly();

        public Group(Guid id, string name, int yearOfEntry)
        {
            Id = id;
            Name = name;
            YearOfEntry = yearOfEntry;
        }

        public void AddStudent(User student)
        {
            if (student.Role != Role.Student)
                throw new InvalidOperationException("Only students can be added to a group.");
            if (!_students.Contains(student))
                _students.Add(student);
        }

        public bool ContainsStudent(User student) => _students.Contains(student);
    }
}

