using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.CourseScheduleRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.CourseScheduleRepository
{
    public class CourseScheduleReadRepository : ReadRepository<CourseSchedule>, ICourseScheduleReadRepository
    {
        public CourseScheduleReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
