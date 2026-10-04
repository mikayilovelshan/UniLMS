using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniLMS.Application.DTOs.ExamResults;
using UniLMS.Application.Interfaces.Services;

namespace UniLMS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExamResultController(IExamResultService _examResultService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateRange([FromBody] CreateExamResultRangeDTO model)
        {
            var result = await _examResultService.CreateRangeAsync(model);

            return StatusCode(200, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRange([FromBody] UpdateExamResultDTO model)
        {
            var result = await _examResultService.UpdateAsync(model);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _examResultService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _examResultService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("soft")]
        public async Task<IActionResult> SoftDelete(List<Guid> ids)
        {
            var result = await _examResultService.SoftDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpDelete("hard")]
        public async Task<IActionResult> HardDelete(List<Guid> ids)
        {
            var result = await _examResultService.HardDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpPost("restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var result = await _examResultService.RestoreAsync(id);
            return Ok(result);
        }
    }
}

