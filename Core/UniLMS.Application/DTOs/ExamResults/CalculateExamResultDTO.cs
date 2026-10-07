using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.ExamResults
{
    public class CalculateExamResultDTO
    {
        public Guid StudentId { get; set; }

        public Guid CourseOfferingId { get; set; }

    }
}
