using API.SignalR;
using Core.IdentityEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

        public TrackingController(ITrackingService trackingService, IHubContext<TrackingHub> hub)
        {
            _trackingService = trackingService;
            _hub = hub;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLocation([FromBody] TrackingDataDto dto)
        {
            try
            {
                Console.WriteLine($"Received: Bus {dto.BusId} at {dto.Latitude}, {dto.Longitude}");

                var enriched = await _trackingService.ProcessTrackingAsync(dto);

                Console.WriteLine("Processed location update");

                string groupName = $"bus_{dto.BusId}";
                await _hub.Clients.Group(groupName).SendAsync("ReceiveLocation", enriched);

                return Ok(enriched);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("driver-tracking-summary")]
        public async Task<IActionResult> GetDriverTrackingSummaries()
        {
            var result = await _trackingService.GetAllDriverTrackingSummariesAsync();
            return Ok(result);
        }
    }
}

