using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.DriverService;
using Services.Services.DriverService.DTO;

namespace SmartTrackingTransport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminDriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly IMapper _mapper;

        public AdminDriverController(IDriverService driverService, IMapper mapper)
        {
            _driverService = driverService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetAllDriversAsync();
            var driverDtos = _mapper.Map<IEnumerable<DriverDto>>(drivers);
            return Ok(driverDtos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            var driver = await _driverService.GetDriverByIdAsync(id);
            if (driver == null) return NotFound();
            var driverDto = _mapper.Map<DriverDto>(driver);
            return Ok(driverDto);
        }


        [HttpPost]
        public async Task<IActionResult> AddDriver(CRUDDriverDto dto)
        {
            var driver = await _driverService.AddDriverAsync(dto);
            var driverDto = _mapper.Map<DriverDto>(driver);
            return Ok(driverDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, CRUDDriverDto dto)
        {
            var success = await _driverService.UpdateDriverAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var success = await _driverService.DeleteDriverAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }


        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetDriverStatus(int id)
        {
            var status = await _driverService.GetDriverStatusAsync(id);
            if (status == null) return NotFound();
            return Ok(status);
        }

            [HttpGet("{driverId}/location")]
            public async Task<IActionResult> GetLastKnownLocation(int driverId)
            {
                var location = await _driverService.GetLastKnownLocationAsync(driverId);
                if (location == null) return NotFound("No recent location found.");
                return Ok(location);
            }
        }
}

