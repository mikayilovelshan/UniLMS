using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.CourseOfferingRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.CourseOfferingRepository
{
    public class CourseOfferingWriteRepository : WriteRepository<CourseOffering>, ICourseOfferingWriteRepository
    {
        public CourseOfferingWriteRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
