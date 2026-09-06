using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Specialities
{
    public class CreateSpecialityDTO
    {
        public string Name { get; set; }

        public string Code  { get; set; }

        public string? Describtion { get; set; }

        public string Degree { get; set; }

        public string? FacultyName { get; set; }

        public string? CafedraName  { get; set; }
    }
}
