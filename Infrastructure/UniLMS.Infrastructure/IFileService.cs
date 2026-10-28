using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Infrastructure
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file);
        void DeleteFile(string filePath);
        Task<(Stream FileStream, string ContentType, string FileName)?> DownLoadFileAsync(string filePath);
    }
}
