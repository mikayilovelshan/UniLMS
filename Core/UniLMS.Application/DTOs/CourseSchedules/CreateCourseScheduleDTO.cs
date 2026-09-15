using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.CourseSchedules
{
    public class CreateCourseScheduleDTO
    {
        public DayOfWeek DayOfWeek { get; set; }

        public Guid CourseOfferingId { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }   
    }
}
