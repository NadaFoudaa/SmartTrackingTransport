using Microsoft.AspNetCore.Mvc;
using Services.Services.TripService;
using Services.Services.TripService.DTO;
using Services.Services.Tripv2Service;
using Services.Services.Tripv2Service.DTO;
using System.Globalization;

namespace SmartTrackingTransport.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class Tripv2Controller : ControllerBase
	{
        private readonly ITripv2Service _tripService;

        public Tripv2Controller(ITripv2Service tripService)
        {
            _tripService = tripService;
        }

        // GET: api/tripv2
        [HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tripv2Dto>>> GetAllTrips()
        {
            var trips = await _tripService.GetAllTripsAsync();
            return Ok(trips);
        }

        // GET: api/tripv2/{id}
        // Calls new service method to get a trip by ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Tripv2Dto>> GetTripById(int id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null)
                return NotFound();

            return Ok(trip);
        }

        // GET: api/tripv2/bybus/{busId}
        // Calls new service method to get trips filtered by BusId
        [HttpGet("bybus/{busId:int}")]
        public async Task<ActionResult<IEnumerable<Tripv2Dto>>> GetTripsByBusId(int busId)
        {
            var trips = await _tripService.GetTripsByBusIdAsync(busId);
            return Ok(trips);
        }

        // GET: api/tripv2/byroute/{route}
        // Calls new service method to get trips filtered by route
        [HttpGet("byroute/{route}")]
        public async Task<ActionResult<IEnumerable<Tripv2Dto>>> GetTripsByRoute(string route)
        {
            var trips = await _tripService.GetTripsByRouteAsync(route);
            return Ok(trips);
        }
        [HttpGet("daily-schedule")]
        public async Task<IActionResult> GetDailySchedule(
            string date = null,
            string origin = null,
            string destination = null,
            bool? isOutbound = null)
        {
            var filter = new RouteScheduleFilter
            {
                Date = string.IsNullOrEmpty(date) ? null : DateOnly.Parse(date, CultureInfo.InvariantCulture),
                Origin = origin,
                Destination = destination,
                IsOutbound = isOutbound
            };

            var schedule = await _tripService.GetDailyScheduleAsync(filter);
            return Ok(schedule);
        }
    }
}
