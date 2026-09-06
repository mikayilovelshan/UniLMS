using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniLMS.Application.DTOs.Faculties;
using UniLMS.Application.Interfaces.Services;

namespace UniLMS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FacultiesController(IFacultyService _facultyService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _facultyService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetbyId(Guid id)
        {
            var result = await  _facultyService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFacultyDTO model)
        {
            var result = await _facultyService.CreateAsync(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateFacultyDTO model)
        {
            var result = await  _facultyService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("soft")]
        public async Task<IActionResult> SoftDelete([FromBody] List<Guid> ids)
        {
            var result = await _facultyService.SoftDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpDelete("hard")]
        public async Task<IActionResult> HardDelete([FromBody] List<Guid> ids)
        {
            var result = await _facultyService.HardDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpPost("restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var result = await _facultyService.RestoreAsync(id);
            return Ok(result);
        }
    }
}
