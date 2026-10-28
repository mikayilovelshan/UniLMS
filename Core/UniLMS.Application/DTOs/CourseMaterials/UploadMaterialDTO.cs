using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.CourseMaterials
{
    public class UploadMaterialDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid CourseId { get; set; }

        public IFormFile File { get; set; }
    }
}
