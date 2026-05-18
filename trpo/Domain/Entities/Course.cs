using System;

namespace QRAttendance.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }

        public Course(Guid id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }
    }
}
