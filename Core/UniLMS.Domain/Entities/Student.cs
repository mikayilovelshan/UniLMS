
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string StudentNumber { get; set; }

        public decimal GPA { get; set; }

        public Guid AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public Guid? GroupId { get; set; }

        public Group? Group { get; set; }  
    }
}
