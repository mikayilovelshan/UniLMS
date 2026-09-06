using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class CourseOffering : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; }    

        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        
        public Guid SemesterId { get; set; }
        public Semester Semester { get; set; }  

        public string RoomCode { get; set; }

    }
}
