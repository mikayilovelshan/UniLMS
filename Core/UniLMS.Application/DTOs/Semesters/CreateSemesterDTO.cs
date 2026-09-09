using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Semesters
{
    public class CreateSemesterDTO
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
