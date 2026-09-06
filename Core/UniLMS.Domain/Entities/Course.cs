using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }    

        public string Code { get; set; }

        public string? Description { get; set; }

        public Guid CafedraId { get; set; }

        public Cafedra Cafedra { get; set; }

        public ICollection<Speciality> Specialities { get; set; }
    }
}
