using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Enums;

namespace UniLMS.Application.DTOs.Specialities
{
    public class UpdateSpecialityDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public SpecialityDegree Degree { get; set; }

        public Guid? FacultyId { get; set; }

        public Guid? CafedraId { get; set; }

        public List<Guid>? CourseIds { get; set; }
    }
}
