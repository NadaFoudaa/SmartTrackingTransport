using Core.Entities;
using Core.Enum;
using Infrastructure.Interfaces;
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
    public class DriverService : IDriverService
    {
        private readonly IGenericRepository<Driver> _driverRepo;
        private readonly IGenericRepository<Trips> _tripRepo;
        private readonly IGenericRepository<TrackingData> _trackingRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DriverService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _driverRepo = _unitOfWork.Repository<Driver>();
            _tripRepo = _unitOfWork.Repository<Trips>();
            _trackingRepo = _unitOfWork.Repository<TrackingData>();
        }

        public async Task<Driver> AddDriverAsync(CRUDDriverDto dto)
        {
            var driver = new Driver
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                LicenseNumber = dto.LicenseNumber,
                Status = DriverStatus.Available
            };

            _driverRepo.Add(driver);
            await _unitOfWork.Complete();
            return driver;
        }

        public async Task<bool> UpdateDriverAsync(int driverId, CRUDDriverDto dto)
        {
            var driver = await _driverRepo.GetByIdAsync(driverId);
            if (driver == null) return false;

            driver.Name = dto.Name;
            driver.PhoneNumber = dto.PhoneNumber;
            driver.LicenseNumber = dto.LicenseNumber;

            _driverRepo.Update(driver);
            await _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> DeleteDriverAsync(int driverId)
        {
            var driver = await _driverRepo.GetByIdAsync(driverId);
            if (driver == null) return false;

            _driverRepo.Delete(driver);
            await _unitOfWork.Complete();
            return true;
        }

        public async Task<Driver> GetDriverByIdAsync(int driverId)
        {
            return await _driverRepo.GetByIdAsync(driverId);
        }

        public async Task<IEnumerable<Driver>> GetAllDriversAsync()
        {
            return await _driverRepo.GetAllAsync();
        }

        public async Task<DriverStatus?> GetDriverStatusAsync(int driverId)
        {
            var driver = await _driverRepo.GetByIdAsync(driverId);
            return driver?.Status;
        }

        public async Task<DriverLocationDto?> GetLastKnownLocationAsync(int driverId)
        {
            var trip = (await _unitOfWork.Repository<Trips>()
                .FindAllAsync(t => t.DriverId == driverId && t.Status == TripStatus.Online && !t.IsDeleted))
                .FirstOrDefault();

            if (trip == null) return null;

            var busId = trip.BusTrips?.FirstOrDefault()?.BusId;
            if (busId == null) return null;

            var latest = (await _unitOfWork.Repository<TrackingData>()
                .FindAllAsync(td => td.BusId == busId.Value))
                .OrderByDescending(td => td.Timestamp)
                .FirstOrDefault();

            if (latest == null) return null;

            return new DriverLocationDto
            {
                DriverId = driverId,
                Latitude = (double)latest.Latitude,
                Longitude = (double)latest.Longitude,
                Timestamp = latest.Timestamp
            };
        }
    }
}