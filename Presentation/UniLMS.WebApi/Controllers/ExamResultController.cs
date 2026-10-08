using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniLMS.Application.DTOs.ExamResults;
using UniLMS.Application.Interfaces.Services;

namespace UniLMS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExamResultController(IExamResultService _examResultService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _examResultService.GetAllAsync();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _examResultService.GetByIdAsync(id);
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("student-summary")]
        public async Task<IActionResult> GetStudentExamResultSummary([FromQuery] Guid studentId, [FromQuery] Guid courseOfferingId)
        {
            var result = await _examResultService.GetStudentExamResultSummary(studentId, courseOfferingId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("course-detailed-report")]
        public async Task<IActionResult> GetStudentCourseDetailedReport([FromQuery] Guid studentId, [FromQuery] Guid courseOfferingId)
        {
            var result = await _examResultService.GetStudentCourseDetailedReport(studentId, courseOfferingId);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("create-range")]
        public async Task<IActionResult> CreateRange([FromBody] CreateExamResultRangeDTO model)
        {
            var result = await _examResultService.CreateRangeAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateExamResultDTO model)
        {
            var result = await _examResultService.UpdateAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("soft/{id:guid}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var result = await _examResultService.SoftDeleteAsync(id);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("soft-range")]
        public async Task<IActionResult> SoftDeleteRange([FromBody] List<Guid> ids)
        {
            var result = await _examResultService.SoftDeleteRangeAsync(ids);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("hard/{id:guid}")]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            var result = await _examResultService.HardDeleteAsync(id);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("hard-range")]
        public async Task<IActionResult> HardDeleteRange([FromBody] List<Guid> ids)
        {
            var result = await _examResultService.HardDeleteRangeAsync(ids);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("restore/{id:guid}")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var result = await _examResultService.RestoreAsync(id);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}