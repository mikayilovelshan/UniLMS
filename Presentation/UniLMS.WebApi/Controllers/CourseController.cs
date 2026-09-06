using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniLMS.Application.DTOs.Courses;
using UniLMS.Application.Interfaces.Services;

namespace UniLMS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CourseController(ICourseService _courseService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCourseDTO model)
        {
            var result = await _courseService.CreateAsync(model);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCourseDTO model)
        {
            var result = await _courseService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("soft")]
        public async Task<IActionResult> SoftDelete(List<Guid> ids)
        {
            var result = await _courseService.SoftDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpDelete("hard")]
        public async Task<IActionResult> HardDelete(List<Guid> ids)
        {
            var result = await _courseService.HardDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _courseService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var result = await _courseService.RestoreAsync(id);
            return Ok(result);
        }

     }       
}
