
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniLMS.Application.DTOs.Attendance;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Application.Interfaces.Services;

namespace UniLMS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AttendanceController(IAttendanceService _attendanceService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateRange([FromBody] CreateAttendanceRangeDTO model)
        {
            var result = await _attendanceService.CreateRangeAsync(model);

            return StatusCode(200, result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRange([FromBody] UpdateAttendanceRangeDTO model)
        {
            var result = await _attendanceService.UpdateRangeAsync(model);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _attendanceService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _attendanceService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("soft")]
        public async Task<IActionResult> SoftDelete(List<Guid> ids)
        {
            var result = await _attendanceService.SoftDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpDelete("hard")]
        public async Task<IActionResult> HardDelete(List<Guid> ids)
        {
            var result = await _attendanceService.HardDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpPost("restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var result = await _attendanceService.RestoreAsync(id);
            return Ok(result);
        }
    }
}
