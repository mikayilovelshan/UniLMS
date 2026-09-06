using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.GroupRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.GroupRepository
{
    public class GroupReadRepository : ReadRepository<Group>, IGroupReadRepository
    {
        public GroupReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
