using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories;
using UniLMS.Application.Repositories.CourseRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.CourseRepository
{
    public class CourseWriteRepository : WriteRepository<Course>, ICourseWriteRepository
    {
        public CourseWriteRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
