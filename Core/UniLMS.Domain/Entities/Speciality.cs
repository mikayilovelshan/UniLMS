using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;
using UniLMS.Domain.Enums;

namespace UniLMS.Domain.Entities
{
    public class Speciality : BaseEntity
    {
        public string Name { get; set; }    

        public string Code { get; set; }

        public SpecialityDegree Degree { get; set; }

        public Guid? FacultyId { get; set; }

        public Faculty? Faculty { get; set; }

        public Guid? CafedraId { get; set; }
        public Cafedra? Cafedra { get; set; }


        public ICollection<Course> Courses { get; set; }
    }
}
