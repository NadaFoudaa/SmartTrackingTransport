using Microsoft.AspNetCore.Mvc;
using Services.Services.TripService;
using Services.Services.TripService.DTO;
using Services.Services.Tripv2Service;
using Services.Services.Tripv2Service.DTO;

namespace SmartTrackingTransport.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TripController : ControllerBase
	{
        private readonly ITripv2Service _tripService;

        public TripController(ITripv2Service tripService)
        {
            _tripService = tripService;
        }

        // GET: api/trip
        [HttpGet]
		public async Task<ActionResult<IEnumerable<Tripv2Dto>>> GetAllTrips()
		{
			var trips = await _tripService.GetAllTripsAsync();
			return Ok(trips);
		}

		// GET: api/trip/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<Tripv2Dto>> GetTripById(int id)
		{
			var trip = await _tripService.GetTripByIdAsync(id);
			if (trip == null) return NotFound();
			return Ok(trip);
		}
	}
}
