using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Courses
{
    public class UpdateCourseDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }    

        public string Code { get; set; }

        public string? Description { get; set; }

        public Guid CafedraId { get; set; }
    }
}
