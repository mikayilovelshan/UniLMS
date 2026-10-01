using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Enums;

namespace UniLMS.Application.DTOs.ExamResults
{
    public class UpdateExamResultDTO
    {
        public Guid Id { get; set; }

        public decimal Score { get; set; }

        public DateTime? EvaluationDate { get; set; }
        
    }
}
