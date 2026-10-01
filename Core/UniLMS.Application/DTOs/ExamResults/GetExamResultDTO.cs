using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.ExamResults
{
    public class GetExamResultDTO
    {
        public Guid Id { get; set; }

        public string StudentFullName { get; set; }

        public string CourseName { get; set; }  

        public string ExamTypeName { get; set; }

        public decimal Score { get; set; }

        public DateTime EvaluationDate { get; set; }
    }
}
