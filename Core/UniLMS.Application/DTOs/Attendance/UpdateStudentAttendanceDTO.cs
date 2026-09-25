using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Attendance
{
    public class UpdateStudentAttendanceDTO
    {
        public Guid AttendanceId { get; set; }

        public bool IsPresent { get; set; }

        public string? Note { get; set; }
    }
}
