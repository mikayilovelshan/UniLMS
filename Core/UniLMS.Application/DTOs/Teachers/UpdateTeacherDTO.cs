using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Teachers
{
    public class UpdateTeacherDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }

        public string ScientificDegree { get; set; }

        public Guid CafedraId { get; set; }
    }
}
