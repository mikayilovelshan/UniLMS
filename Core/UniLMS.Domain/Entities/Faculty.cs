using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class Faculty : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }    

        public string? Description { get; set; }

        public ICollection<Cafedra> Cafedras { get; set; }

        public ICollection<Speciality> Specialities { get; set; }

        
    }
}
