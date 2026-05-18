using System;
using System.Collections.Generic;
using QRAttendance.Domain.Entities;

namespace QRAttendance.DataAccess.Interfaces
{
    public interface ICourseRepository
    {
        Course GetById(Guid id);
        IEnumerable<Course> GetAll();
        void Add(Course entity);
        void Update(Course entity);
        void Delete(Guid id);
    }
}
