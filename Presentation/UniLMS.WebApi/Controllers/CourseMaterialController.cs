using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniLMS.Application.DTOs.CourseMaterials;
using UniLMS.Domain.Entities;
using UniLMS.Infrastructure;
using UniLMS.Persistence.Contexts;

namespace UniLMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMaterialController(IFileService _fileService,
                    AppDbContext _context) : ControllerBase
    {
        [HttpPost("upload")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> UploadMaterial([FromForm] UploadMaterialDTO model)
        {
            var uploadResult = await _fileService.UploadFileAsync(model.File);

            var material = new CourseMaterial
            {
                Title = model.Title,
                Description = model.Description,
                CourseId = model.CourseId,
                FileName = model.File.FileName,
                FilePath = uploadResult,
                CreatedDate = DateTime.UtcNow
            };

            _context.CourseMaterials.Add(material);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Material uploaded successfully.",
                materialId = material.Id,
                filePath = uploadResult
            });
        }

        [HttpGet("download")]
        [Authorize(Roles = "Student, Teacher, Admin")]

        public async Task<IActionResult> DownloadMaterial([FromQuery] string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return BadRequest(new { message = "File path is required." });

            var fileResult = await _fileService.DownLoadFileAsync(filePath);

            if (fileResult == null)
                return NotFound("Fayl tapılmadı və ya artıq silinib.");

            var (fileStream, contentType, fileName) = fileResult.Value;

            return File(fileStream, contentType, fileName);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> DeleteMaterial([FromQuery] string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
                return BadRequest(new { message = "File path is required." });

            var material = await _context.CourseMaterials.FirstOrDefaultAsync(x => x.FilePath == filePath);

            if (material == null)
                return NotFound("Bu ünvana uyğun material bazada tapılmadı.");

            _fileService.DeleteFile(filePath);

            _context.CourseMaterials.Remove(material);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Material və fiziki fayl uğurla silindi." });


        }
    }
}
