using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Enums;

namespace UniLMS.Application.DTOs.ExamResults
{
    public  class CreateExamResultRangeDTO
    {

        public Guid CourseOfferingId { get; set; }

        public ExamType ExamType { get; set; }

        public DateTime? EvaluationDate  { get; set; }

        public List<CreateStudentExamResultDTO> StudentScores { get; set; }
    }
}
