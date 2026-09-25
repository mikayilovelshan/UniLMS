using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Attendance
{
    public class GetAttendanceDTO
    {
        public Guid CourseScheduleId { get; set; }

        public string CourseName { get; set; }

        public string GroupName { get; set; }

        public DateTime Date { get; set; }

        public List<GetStudentAttendanceDTO> Students { get; set; }
    }
}
