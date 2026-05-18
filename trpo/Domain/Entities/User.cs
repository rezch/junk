using System;
using QRAttendance.Domain.Enums;

namespace QRAttendance.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public Role Role { get; private set; }
        public bool IsActive { get; private set; }

        public User(Guid id, string name, string email, Role role)
        {
            Id = id;
            Name = name;
            Email = email;
            Role = role;
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;
    }
}
