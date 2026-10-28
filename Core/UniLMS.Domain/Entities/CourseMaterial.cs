using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class CourseMaterial : BaseEntity
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public Guid CourseId { get; set; }

        public Course Course { get; set; }

    }
}
