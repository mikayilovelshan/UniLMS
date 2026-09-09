using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.TeacherRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.TeacherRepository
{
    public class TeacherReadRepository : ReadRepository<Teacher>, ITeacherReadRepository
    {
        public TeacherReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
