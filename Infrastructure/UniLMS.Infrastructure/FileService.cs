using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using UniLMS.Application.Utilities.Results;

namespace UniLMS.Infrastructure
{
    public class FileService(IWebHostEnvironment _env) : IFileService
    {
        public void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Fayl seçilməyib və ya boşdur.");

            var fullPath = Path.Combine(_env.WebRootPath, filePath);

            if(File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task<(Stream FileStream, string ContentType, string FileName)?> DownLoadFileAsync(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
                throw new ArgumentException("Fayl seçilməyib və ya boşdur.");

            var fullpath = Path.Combine(_env.WebRootPath, filePath);

            if (!File.Exists(fullpath))
                return null;

            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(fullpath, FileMode.Open, FileAccess.Read))
            {
                await stream.CopyToAsync(memoryStream);
            }

            memoryStream.Position = 0;

            var fileName = Path.GetFileName(fullpath);
            var contentType = "application/octet-stream";

            return (memoryStream, contentType, fileName);
            

        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Fayl seçilməyib və ya boşdur.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "materials");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var physicalFilePath = Path.Combine(uploadsFolder, uniqueFileName);

            using(var fileStream = new FileStream(physicalFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine("uploads", "materials", uniqueFileName).Replace("\\", "/");
        }
    }
}
