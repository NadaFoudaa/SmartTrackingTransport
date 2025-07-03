using Microsoft.AspNetCore.Mvc;
using Services.Services.SeatService;
using Services.Services.SeatService.DTO;

namespace SmartTrackingTransport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetSeatLayout(int tripId)
        {
            int userId = 1; // Normally from token
            var layout = await _seatService.GetSeatLayoutAsync(tripId, userId);
            if (layout == null) return NotFound("Trip not found.");
            return Ok(layout);
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveSeat([FromBody] ReserveSeatDto dto)
        {
            int userId = 1; // Normally from token
            var result = await _seatService.ReserveSeatAsync(dto, userId);

            return result == "Seat reserved successfully."
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
