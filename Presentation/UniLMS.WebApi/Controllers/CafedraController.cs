using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniLMS.Application.DTOs.Cafedras;
using UniLMS.Application.DTOs.Faculties;
using UniLMS.Application.Interfaces.Services;

namespace UniLMS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CafedraController(ICafedraService _cafedraService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCafedraDTO model)
        {
            var result = await _cafedraService.CreateAsync(model);

            return StatusCode(200, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCafedraDTO model)
        {
            var result = await _cafedraService.UpdateAsync(model);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _cafedraService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _cafedraService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpDelete("soft")]
        public async Task<IActionResult> SoftDelete(List<Guid> ids)
        {
            var result = await _cafedraService.SoftDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpDelete("hard")]
        public async Task<IActionResult> HardDelete(List<Guid> ids)
        {
            var result = await _cafedraService.HardDeleteRangeAsync(ids);
            return Ok(result);
        }

        [HttpPost("restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var result = await _cafedraService.RestoreAsync(id);
            return Ok(result);
        }
    }
}
