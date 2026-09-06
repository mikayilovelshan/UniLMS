using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public Guid StudentId {  get; set; }

        public Student Student { get; set; }

        public Guid CourseScheduleId { get; set; }
        public CourseSchedule CourseSchedule { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; } 
        public string? Note   { get; set; }
    }
}
