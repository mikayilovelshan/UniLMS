using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.CourseSchedules
{
    public class GetCourseScheduleDTO
    {
        public Guid Id { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public Guid CourseOfferingId { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string CourseName { get; set; }

        public string TeacherName { get; set; }

        public string GroupName { get; set; }

        public string RoomCode { get; set; }
    }
}
