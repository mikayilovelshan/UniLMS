using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities.Common;

namespace UniLMS.Domain.Entities
{
    public class Semester : BaseEntity
    {
        public string Name { get; set; }

        public DateTime StartDate  { get; set; }
        
        public DateTime EndDate  { get; set; }

        public bool IsActive { get; set; }

    }
}
