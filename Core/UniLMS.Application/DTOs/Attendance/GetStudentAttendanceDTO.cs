using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Attendance
{
    public class GetStudentAttendanceDTO
    {
        public Guid AttendanceId { get; set; }

        public Guid StudentId { get; set; }

        public string StudentFullName { get; set; }

        public bool IsPresent { get; set; }

        public string? Note {  get; set; }
    }
}
