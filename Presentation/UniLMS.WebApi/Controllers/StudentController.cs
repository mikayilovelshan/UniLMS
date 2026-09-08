using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniLMS.Application.DTOs.Students;
using UniLMS.Application.Interfaces.Services;


namespace UniLMS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController(IStudentService _studentService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateStudentDTO model)
        {
            var result = await _studentService.CreateAsync(model);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateStudentDTO model)
        {
            var result = await _studentService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("soft")]
        public async Task<IActionResult> SoftDelete(List<Guid> ids)
        {
            var result = await _studentService.SoftDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpDelete("hard")]
        public async Task<IActionResult> HardDelete(List<Guid> ids)
        {
            var result = await _studentService.HardDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studentService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _studentService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var result = await _studentService.RestoreAsync(id);
            return Ok(result);
        }
    }
}

