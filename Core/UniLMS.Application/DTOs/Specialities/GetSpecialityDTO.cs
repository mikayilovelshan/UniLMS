using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Courses;
using UniLMS.Domain.Enums;

namespace UniLMS.Application.DTOs.Specialities
{
    public class GetSpecialityDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public SpecialityDegree Degree { get; set; }

        public string? FacultyName { get; set; }

        public string? CafedraName { get; set; }

        public List<GetCourseDTO> Courses { get; set; }
    }
}
