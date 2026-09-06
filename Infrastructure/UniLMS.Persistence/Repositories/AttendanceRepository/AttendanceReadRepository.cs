using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.AttendanceRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.AttendanceRepository
{
    public class AttendanceReadRepository : ReadRepository<Attendance>, IAttendanceReadRepository
    {
        public AttendanceReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
