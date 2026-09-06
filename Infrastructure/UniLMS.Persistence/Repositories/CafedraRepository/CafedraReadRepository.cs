using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.CafedraRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.CafedraRepository
{
    public class CafedraReadRepository : ReadRepository<Cafedra>, ICafedraReadRepository
    {
        public CafedraReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
