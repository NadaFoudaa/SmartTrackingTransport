using AutoMapper;
using Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.TripService;
using Services.Services.TripService.DTO;
using Services.Services.Tripv2Service;
using Services.Services.Tripv2Service.DTO;

namespace SmartTrackingTransport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminTripController : ControllerBase
    {
        private readonly ITripv2Service _tripService;
        private readonly IMapper _mapper;

        public AdminTripController(ITripv2Service tripService, IMapper mapper)
        {
            _tripService = tripService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrip(CRUDTripDto dto)
        {
            var trip = await _tripService.CreateTripAsync(dto);
            var tripDto = _mapper.Map<Tripv2Dto>(trip);
            return Ok(tripDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, CRUDTripDto dto)
        {
            var success = await _tripService.UpdateTripAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var success = await _tripService.DeleteTripAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTripStatus(int id, [FromQuery] TripStatus status)
        {
            var success = await _tripService.SetTripStatusAsync(id, status);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{tripId}/assign-driver/{driverId}")]
        public async Task<IActionResult> AssignDriver(int tripId, int driverId)
        {
            var success = await _tripService.AssignDriverToTripAsync(tripId, driverId);
            if (!success) return BadRequest("Driver already assigned to another active trip.");
            return Ok();
        }

        [HttpDelete("{tripId}/unassign-driver")]
        public async Task<IActionResult> UnassignDriver(int tripId)
        {
            var success = await _tripService.UnassignDriverFromTripAsync(tripId);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrips()
        {
            var trips = await _tripService.GetAllTripsAsync();
            var tripDtos = _mapper.Map<IEnumerable<Tripv2Dto>>(trips);
            return Ok(tripDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTripById(int id)
        {
            var trip = await _tripService.GetTripByIdAsync(id);
            if (trip == null) return NotFound();
            var tripDto = _mapper.Map<Tripv2Dto>(trip);
            return Ok(tripDto);
        }
    }
}