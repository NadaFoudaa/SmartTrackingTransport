using AutoMapper;
using Infrastucture.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Services.BusService;
using Services.Services.BusService.DTO;
using Services.Services.Busv2Service;
using Services.Services.Busv2Service.DTO;

namespace SmartTrackingTransport.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BusController : ControllerBase
	{
        private readonly IBusv2Service _busService;

        public BusController(IBusv2Service busService)
        {
            _busService = busService;
        }
        [HttpGet("Buses")]
		public async Task<ActionResult<IEnumerable<Busv2Dto>>> GetAllBuses()
		{
			var buses = await _busService.GetAll();
			return Ok(buses);
		}
		[HttpGet("{busId}/abstract")]
		public async Task<ActionResult<Busv2AbstractDto>> GetBusAbstract(int busId)
		{
			var busAbstract = await _busService.GetBusAbstractAsync(busId);
			if (busAbstract == null)
				return NotFound();

			return Ok(busAbstract);
		}
		[HttpGet("{busId}/trip-details")]
		public async Task<ActionResult<Busv2TripDetailsDto>> GetBusTripDetails(int busId)
		{
			var tripDetails = await _busService.GetBusTripDetailsAsync(busId);
			if (tripDetails == null)
				return NotFound();

			return Ok(tripDetails);
		}


		[HttpGet("GetBusesFromOrginToDestination")]
		public async Task<ActionResult<IEnumerable<Busv2Dto>>> GetBuses([FromQuery] string origin, [FromQuery] string destination)
		{
			var buses = await _busService.GetAvailableBusesAsync(origin, destination);
			return Ok(buses);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Busv2Dto>> GetBus(int id)
		{
			var bus = await _busService.GetBusByIdAsync(id);
			if (bus == null)
				return NotFound();

			return Ok(bus);
		}
		
		[HttpGet("{busNumber}/trips-from-origin")]
		public async Task<ActionResult<Busv2TripsDto>> GetBusTripsFromOrigin(
			string busNumber,
			[FromQuery] string origin,
			[FromQuery] DateTime? date = null)
		{
			if (string.IsNullOrEmpty(busNumber) || string.IsNullOrEmpty(origin))
				return BadRequest(new { message = "Bus number and origin are required" });

			var searchDate = date ?? DateTime.Today;
			var trips = await _busService.GetBusTripsFromOriginAsync(busNumber, origin, searchDate);

			if (trips == null)
				return NotFound(new { message = $"Bus with number {busNumber} not found" });

			return Ok(trips);
		}

		[HttpGet("{busNumber}/trips-to-destination")]
		public async Task<ActionResult<Busv2TripsDto>> GetBusTripsToDestination(
			string busNumber,
			[FromQuery] string destination,
			[FromQuery] DateTime? date = null)
		{
			if (string.IsNullOrEmpty(busNumber) || string.IsNullOrEmpty(destination))
				return BadRequest(new { message = "Bus number and destination are required" });

			var searchDate = date ?? DateTime.Today;
			var trips = await _busService.GetBusTripsToDestinationAsync(busNumber, destination, searchDate);

			if (trips == null)
				return NotFound(new { message = $"Bus with number {busNumber} not found" });

			return Ok(trips);
		}

	}
}