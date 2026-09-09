using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.TeacherRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.TeacherRepository
{
    public class TeacherWriteRepository : WriteRepository<Teacher>, ITeacherWriteRepository
    {
        public TeacherWriteRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
