using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.ExamResults
{
    public class StudentExamResultSummaryDTO
    {
        public Guid StudentId { get; set; }

        public string StudentFullName { get; set; }

        public Guid CourseOfferingId { get; set; }

        public string CourseName { get; set; }

        public decimal EntryScore { get; set; }

        public decimal FinalScore { get; set; }

        public decimal TotalScore  => EntryScore + FinalScore; 

        public string LetterGrade {  get; set; }

        public bool IsPassed { get; set; }

    }
}
