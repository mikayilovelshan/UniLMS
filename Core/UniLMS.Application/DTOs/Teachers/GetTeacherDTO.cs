using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Teachers
{
    public class GetTeacherDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ScientificDegree { get; set; }

        public string CafedraName { get; set; }

    }
}
