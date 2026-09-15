using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.CourseOfferingRepository;
using UniLMS.Application.Repositories.CourseScheduleRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories
{
    public class CourseScheduleWriteRepository : WriteRepository<CourseSchedule>, ICourseScheduleWriteRepository
    {
        public CourseScheduleWriteRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
