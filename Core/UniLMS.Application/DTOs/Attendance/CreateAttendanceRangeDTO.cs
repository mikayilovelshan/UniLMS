using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Attendance
{
    public class CreateAttendanceRangeDTO
    {
        public Guid CourseScheduleId { get; set; }

        public DateTime Date {  get; set; }

        public List<CreateStudentAttendanceDTO> Students { get; set; }
    }
}
