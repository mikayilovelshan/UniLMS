using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Courses
{
    public class GetCourseDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public string CafedraName { get; set; }

        
    }
}
