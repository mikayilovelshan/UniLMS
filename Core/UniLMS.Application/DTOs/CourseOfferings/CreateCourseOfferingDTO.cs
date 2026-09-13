using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.CourseOfferings
{
    public class CreateCourseOfferingDTO
    {
        public Guid SemesterId { get; set; }

        public Guid GroupId { get; set; }

        public Guid CourseId { get; set; }

        public Guid TeacherId { get; set; }

        public string RoomCode { get; set; }
    }
}
