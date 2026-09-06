using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.FacultyRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.FacultyRepository
{
    public class FacultyReadRepository : ReadRepository<Faculty>, IFacultyReadRepository
    {
        public FacultyReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
