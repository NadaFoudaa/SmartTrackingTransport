using Microsoft.AspNetCore.Mvc;
using Services.Services.TripService.DTO;
using Services.Services.TripService;

namespace SmartTrackingTransport.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TripController : ControllerBase
	{
		private readonly ITripService _tripService;

		public TripController(ITripService tripService)
		{
			_tripService = tripService;
		}

		// GET: api/trip
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TripDto>>> GetAllTrips()
		{
			var trips = await _tripService.GetAllTripsAsync();
			return Ok(trips);
		}

		// GET: api/trip/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<TripDto>> GetTripById(int id)
		{
			var trip = await _tripService.GetTripByIdAsync(id);
			if (trip == null) return NotFound();
			return Ok(trip);
		}
	}
}
