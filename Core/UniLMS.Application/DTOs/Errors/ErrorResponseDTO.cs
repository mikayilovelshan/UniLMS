using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Application.DTOs.Errors
{
    public class ErrorResponseDTO
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public List<string>? Errors { get; set; } 
    }
}
