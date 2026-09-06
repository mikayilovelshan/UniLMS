using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class StudentGrade : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }   

        public Guid CourseOfferingId { get; set; }
        public CourseOffering CourseOffering { get; set; }

        public decimal DailyScore { get; set; }
        public decimal AttendanceScore {  get; set; }

        public decimal IndependentWorkScore { get; set; }

        public decimal ColloquiumScore   { get; set; }

        public decimal EntryScore { get; set; }

        public decimal FinalExamScore { get; set; }

        public decimal TotalScore   { get; set; }

        public string LetterGrade {  get; set; }


        public bool IsPassed {  get; set; }
    }
}
