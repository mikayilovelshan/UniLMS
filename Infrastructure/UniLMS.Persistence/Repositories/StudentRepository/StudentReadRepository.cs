using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.StudentRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.StudentRepository
{
    public class StudentReadRepository : ReadRepository<Student>, IStudentReadRepository
    {
        public StudentReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
