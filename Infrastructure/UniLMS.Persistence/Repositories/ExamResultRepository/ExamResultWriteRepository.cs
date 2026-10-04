using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Repositories.ExamResultRepository;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;

namespace UniLMS.Persistence.Repositories.ExamResultRepository
{
    public class ExamResultWriteRepository : WriteRepository<ExamResult>, IExamResultWriteRepository
    {
        public ExamResultWriteRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
