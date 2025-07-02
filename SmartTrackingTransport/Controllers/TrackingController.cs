using API.SignalR;
using Core.IdentityEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Services.Services.TrackingService;
using Services.Services.TrackingService.DTO;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingService _trackingService;
        private readonly IHubContext<TrackingHub> _hub;
        private readonly ILogger<TrackingController> _logger;

        public TrackingController(ITrackingService trackingService, IHubContext<TrackingHub> hub, ILogger<TrackingController> logger)
        {
            _trackingService = trackingService;
            _hub = hub;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLocation([FromBody] TrackingDataDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var timestamp = DateTime.UtcNow;
                var trackingDto = await _trackingService.QuickUpdateBusLocationAsync(dto, timestamp);

                try
                {
                    await _hub.Clients.Group(SignalRGroups.BusGroup(dto.BusId))
                        .SendAsync("ReceiveLocation", trackingDto);
                }
                catch (Exception hubEx)
                {
                    _logger.LogWarning(hubEx, "SignalR push failed for BusId {BusId}", dto.BusId);
                }

                return Ok(trackingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update bus location for BusId {BusId}", dto.BusId);
                return StatusCode(500, new { error = "Internal server error. Please try again later." });
            }
        }
        [HttpGet("{busId}")]
        public async Task<IActionResult> GetBusCurrentLocation(int busId)
        {
            var bus = await _trackingService.GetBusCurrentTrackingAsync(busId);

            if (bus == null)
                return NotFound("Bus not found or no tracking data");

            return Ok(bus);
        }

        [HttpGet("{busId}/location")]
        public async Task<IActionResult> GetBusLocation(int busId)
        {
            var location = await _trackingService.GetBusLocationOnlyAsync(busId);

            if (location == null)
                return NotFound("Bus not found");

            return Ok(location);
        }
        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyBuses([FromQuery] decimal latitude, [FromQuery] decimal longitude, [FromQuery] double radiusMeters = 1000)
        {
            var buses = await _trackingService.GetNearbyBusesAsync(latitude, longitude, radiusMeters);

            return Ok(buses);
        }
    }
}
