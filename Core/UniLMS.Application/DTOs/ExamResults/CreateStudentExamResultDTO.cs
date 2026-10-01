using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.ExamResults
{
    public class CreateStudentExamResultDTO
    {
        public Guid StudentId { get; set; }

        public decimal Score { get; set; }
    }
}
