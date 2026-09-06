using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class CourseSchedule : BaseEntity
    {
        public DayOfWeek DayOfWeek { get; set; }

        public Guid CourseOfferingId { get; set; }

        public CourseOffering CourseOffering { get; set; }

        public TimeSpan StartTime { get; set; }
        
        public TimeSpan EndTime   { get; set; }


    }
}
