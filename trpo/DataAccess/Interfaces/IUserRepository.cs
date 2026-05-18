using System;
using System.Collections.Generic;
using QRAttendance.Domain.Entities;

namespace QRAttendance.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        User GetById(Guid id);
        IEnumerable<User> GetAll();
        void Add(User entity);
        void Update(User entity);
        void Delete(Guid id);
    }
}
