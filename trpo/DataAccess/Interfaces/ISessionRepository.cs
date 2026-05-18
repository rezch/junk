using System;
using System.Collections.Generic;
using QRAttendance.Domain.Entities;

namespace QRAttendance.DataAccess.Interfaces
{
    public interface ISessionRepository
    {
        Session GetById(Guid id);
        IEnumerable<Session> GetAll();
        void Add(Session entity);
        void Update(Session entity);
        void Delete(Guid id);
    }
}
