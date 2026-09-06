using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Enums;

namespace UniLMS.Application.DTOs.Specialities
{
    public class CreateSpecialityDTO
    {
        public string Name { get; set; }

        public string Code  { get; set; }

        public string? Describtion { get; set; }

        public SpecialityDegree Degree { get; set; }

        public Guid? FacultyId { get; set; }

        public Guid? CafedraId  { get; set; }

        public List<Guid>? CourseIds { get; set; }
    }
}
