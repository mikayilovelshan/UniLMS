using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Code { get; set; }    

        public Guid SpecialityId { get; set; }

        public Speciality Speciality { get; set; }

        public ICollection<Student> Students { get; set; }

       

    }
}
