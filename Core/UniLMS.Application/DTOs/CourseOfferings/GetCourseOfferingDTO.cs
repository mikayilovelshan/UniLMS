using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.CourseOfferings
{
    public class GetCourseOfferingDTO
    {
        public Guid Id { get; set; }

        public string SemesterName { get; set; }

        public string GroupCode { get; set; }

        public string CourseName { get; set; }

        public string TeacherName { get; set; }

        public string RoomCode { get; set; }
    }
}
