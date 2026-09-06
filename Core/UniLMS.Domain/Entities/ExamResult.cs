using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;
using UniLMS.Domain.Enums;

namespace UniLMS.Domain.Entities
{
    public class ExamResult : BaseEntity
    {
        public Guid StudentId { get; set; }

        public Student Student { get; set; }

        public Guid CourseOfferingId { get; set; }

        public CourseOffering CourseOffering { get; set; }

        public ExamType ExamType { get; set; }  

        public decimal Score { get; set; }

        public DateTime EvaluationDate { get; set; }

    }
}
