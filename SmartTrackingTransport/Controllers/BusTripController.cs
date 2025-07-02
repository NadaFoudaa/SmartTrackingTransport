using Microsoft.AspNetCore.Mvc;
using Services.Services.BusTripService;
using Services.Services.BusTripService.DTO;

namespace SmartTrackingTransport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusTripController : ControllerBase
    {
        private readonly IBusTripService _busTripService;

        public BusTripController(IBusTripService busTripService)
        {
            _busTripService = busTripService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusTripDto>>> GetAll()
        {
            var result = await _busTripService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{busId:int}/{tripId:int}")]
        public async Task<ActionResult<BusTripDto>> GetByIds(int busId, int tripId)
        {
            var result = await _busTripService.GetByIdsAsync(busId, tripId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BusTripDto dto)
        {
            await _busTripService.AddAsync(dto);
            return Ok();
        }

        [HttpDelete("{busId:int}/{tripId:int}")]
        public async Task<IActionResult> Delete(int busId, int tripId)
        {
            await _busTripService.DeleteAsync(busId, tripId);
            return NoContent();
        }
        [HttpPost("{tripId}/assign-bus/{busId}")]
        public async Task<IActionResult> AssignBusToTrip(int tripId, int busId)
        {
            var success = await _busTripService.AssignBusToTripAsync(tripId, busId);
            if (!success) return BadRequest("Assignment failed: trip or bus not found, or already assigned.");
            return Ok("Bus successfully assigned to trip.");
        }
        [HttpDelete("{tripId}/unassign-bus/{busId}")]
        public async Task<IActionResult> UnassignBusFromTrip(int tripId, int busId)
        {
            var success = await _busTripService.UnassignBusFromTripAsync(tripId, busId);
            if (!success) return NotFound("Bus was not assigned to this trip or doesn't exist.");

            return Ok("Bus unassigned successfully.");
        }
    }
}
