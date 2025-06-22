using AutoMapper;
using Infrastucture.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Services.BusService;
using Services.Services.BusService.DTO;

namespace SmartTrackingTransport.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BusController : ControllerBase
	{
		private readonly IBusService _busService;

		public BusController(IBusService busService)
		{
			_busService = busService;
		}
		[HttpGet("Buses")]
		public async Task<ActionResult<IEnumerable<BusDto>>> GetAllBuses()
		{
			var buses = await _busService.GetAll();
			return Ok(buses);
		}
		[HttpGet("{busId}/abstract")]
		public async Task<ActionResult<BusAbstractDto>> GetBusAbstract(int busId)
		{
			var busAbstract = await _busService.GetBusAbstractAsync(busId);
			if (busAbstract == null)
				return NotFound();

			return Ok(busAbstract);
		}
		[HttpGet("{busId}/trip-details")]
		public async Task<ActionResult<BusTripDetailsDto>> GetBusTripDetails(int busId)
		{
			var tripDetails = await _busService.GetBusTripDetailsAsync(busId);
			if (tripDetails == null)
				return NotFound();

			return Ok(tripDetails);
		}


		[HttpGet("GetBusesFromOrginToDestination")]
		public async Task<ActionResult<IEnumerable<BusDto>>> GetBuses([FromQuery] string origin, [FromQuery] string destination)
		{
			var buses = await _busService.GetAvailableBusesAsync(origin, destination);
			return Ok(buses);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BusDto>> GetBus(int id)
		{
			var bus = await _busService.GetBusByIdAsync(id);
			if (bus == null)
				return NotFound();

			return Ok(bus);
		}
		
		[HttpGet("{busNumber}/trips-from-origin")]
		public async Task<ActionResult<BusTripsDto>> GetBusTripsFromOrigin(
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
		public async Task<ActionResult<BusTripsDto>> GetBusTripsToDestination(
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
		/*
		[HttpGet("nearby")]
		public async Task<ActionResult<IEnumerable<BusDto>>> GetBusesNearLocation(
			[FromQuery] decimal latitude,
			[FromQuery] decimal longitude,
			[FromQuery] double radius = 5.0)
		{
			if (radius <= 0 || radius > 50)
				return BadRequest(new { message = "Radius must be between 0 and 50 km" });

			var buses = await _busService.GetBusesNearLocationAsync(latitude, longitude, radius);
			return Ok(buses);
		}
		*/

	}
}