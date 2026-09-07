using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Students
{
    public class UpdateStudentDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }    

        public string StudentNumber { get; set; }

        public string Email { get; set; }

        public Guid GroupId { get; set; }

    }
}
