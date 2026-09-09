using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Groups
{
    public class CreateGroupDTO
    {
        public string Code { get; set; }

        public Guid SpecialityId { get; set; }

        public List<Guid>? StudentIds { get; set; } 
    }
}
