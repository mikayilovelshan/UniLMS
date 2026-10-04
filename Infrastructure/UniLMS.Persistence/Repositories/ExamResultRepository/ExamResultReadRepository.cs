using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.ExamResultRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.ExamResultRepository
{
    public class ExamResultReadRepository : ReadRepository<ExamResult>, IExamResultReadRepository
    {
        public ExamResultReadRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
