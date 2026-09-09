using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Semesters
{
    public class GetSemesterDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
       
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
