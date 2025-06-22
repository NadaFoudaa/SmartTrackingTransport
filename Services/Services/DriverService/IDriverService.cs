using Core.Enum;
using Infrastucture.Entities;
using Services.Services.DriverService.DTO;
using Services.Services.Tripv2Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.DriverService
{
    public interface IDriverService
    {
        Task<Driver> AddDriverAsync(CRUDDriverDto dto);
        Task<bool> UpdateDriverAsync(int driverId, CRUDDriverDto dto);
        Task<bool> DeleteDriverAsync(int driverId);
        Task<Driver> GetDriverByIdAsync(int driverId);
        Task<IEnumerable<Driver>> GetAllDriversAsync();
        Task<DriverStatus?> GetDriverStatusAsync(int driverId);
        Task<DriverLocationDto?> GetLastKnownLocationAsync(int driverId);

    }

}
