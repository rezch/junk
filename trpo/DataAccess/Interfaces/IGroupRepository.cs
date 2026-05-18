using System;
using System.Collections.Generic;
using QRAttendance.Domain.Entities;

namespace QRAttendance.DataAccess.Interfaces
{
    public interface IGroupRepository
    {
        Group GetById(Guid id);
        IEnumerable<Group> GetAll();
        void Add(Group entity);
        void Update(Group entity);
        void Delete(Guid id);
    }
}
