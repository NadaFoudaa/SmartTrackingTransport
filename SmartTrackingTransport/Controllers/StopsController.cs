using Microsoft.AspNetCore.Mvc;
using Services.Services.StopsService;
using Services.Services.StopsService.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StopsController : ControllerBase
	{
		private readonly IStopsService _stopsService;

		public StopsController(IStopsService stopsService)
		{
			_stopsService = stopsService;
		}

		// GET: api/stops/route/5
		[HttpGet("route/{routeId}")]
		public async Task<ActionResult<IEnumerable<StopsDto>>> GetStopsByRoute(int routeId)
		{
			var stops = await _stopsService.GetAllStopsForRouteAsync(routeId);
			return Ok(stops);
		}

		// GET: api/stops/5
		[HttpGet("{stopId}")]
		public async Task<ActionResult<StopsDto>> GetStopById(int stopId)
		{
			var stop = await _stopsService.GetStopByIdAsync(stopId);
			if (stop == null)
			{
				return NotFound();
			}
			return Ok(stop);
		}
	}
}
