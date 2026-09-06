using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.CourseRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.CourseRepository
{
    public class CourseReadRepository : ReadRepository<Course>, ICourseReadRepository
    {
        public CourseReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
