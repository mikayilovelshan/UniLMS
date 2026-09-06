using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.SemesterRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.SemesterRepository
{
    public class SemesterWriteRepository : WriteRepository<Semester>, ISemesterWriteRepository
    {
        public SemesterWriteRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
