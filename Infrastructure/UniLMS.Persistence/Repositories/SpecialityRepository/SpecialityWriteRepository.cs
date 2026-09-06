using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.SpecialityRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.SpecialityRepository
{
    public class SpecialityWriteRepository : WriteRepository<Speciality>, ISpecialityWriteRepository
    {
        public SpecialityWriteRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
