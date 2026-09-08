using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Groups
{
    public class GetGroupDTO
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string SpecialityName { get; set; }

        public List<GroupStudentDTO> Students { get; set; }
    }

    public class GroupStudentDTO
    {
        public Guid Id { get; set; }
        public string StudentNumber { get; set; }
        public string FullName { get; set; }
    }
}
