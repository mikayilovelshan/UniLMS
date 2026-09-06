using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class Teacher : BaseEntity
    {
        public string ScientificDegree { get; set; }

        public Guid AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public Guid CafedraId { get; set; }

        public Cafedra Cafedra { get; set; } 
    }
}
