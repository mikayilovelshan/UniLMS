using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Domain.Entities;

namespace UniLMS.Application.DTOs.Cafedras
{
    public class GetCafedraDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }    

        public string Code { get; set; }

        public string? Description { get; set; }

        public Guid FacultyId { get; set; }

        public string FacultyName  { get; set; }
    }
}
