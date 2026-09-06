using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class Cafedra : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }    

        public string? Description { get; set; }    

        public Guid FacultyId { get; set; }

        public Faculty Faculty { get; set; }    

        public ICollection<Speciality> Specialities { get; set; }

        public ICollection<Course> Courses { get; set; }

    }
}
