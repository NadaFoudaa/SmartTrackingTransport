using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Busv2Service;
using Services.Services.Busv2Service.DTO;

namespace SmartTrackingTransport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminBusController : ControllerBase
    {
        private readonly IBusv2Service _busService;
        private readonly IMapper _mapper;

        public AdminBusController(IBusv2Service busService, IMapper mapper)
        {
            _busService = busService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBus(Busv2Dto dto)
        {
            var success = await _busService.AddBusAsync(dto);
            if (!success) return BadRequest("Bus creation failed.");
            return Ok(dto); // Or return the created bus with ID if needed
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBus(int id, Busv2Dto dto)
        {
            var success = await _busService.UpdateBusAsync(id, dto);
            if (!success) return NotFound("Bus not found.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var success = await _busService.RemoveBusAsync(id);
            if (!success) return NotFound("Bus not found.");
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBusStatus(int id, [FromQuery] string status)
        {
            var success = await _busService.UpdateBusStatusAsync(id, status);
            if (!success) return NotFound("Bus not found.");
            return NoContent();
        }

        [HttpPost("assign-driver")]
        public async Task<IActionResult> AssignDriverToBus([FromBody] AssignDriverDto dto)
        {
            var success = await _busService.AssignDriverAsync(dto);
            if (!success) return BadRequest("Driver already assigned or invalid.");
            return Ok("Driver assigned successfully.");
        }
        [HttpPost("unassign-driver")]
        public async Task<IActionResult> UnassignDriver([FromBody] UnassignDriverDto dto)
        {
            var success = await _busService.UnassignDriverAsync(dto);
            if (!success) return BadRequest("Driver already unassigned or bus not found.");
            return Ok("Driver unassigned.");
        }
    }
}

