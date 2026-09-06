using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.SpecialityRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.SpecialityRepository
{
    public class SpecialityReadRepository : ReadRepository<Speciality>, ISpecialityReadRepository
    {
        public SpecialityReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
