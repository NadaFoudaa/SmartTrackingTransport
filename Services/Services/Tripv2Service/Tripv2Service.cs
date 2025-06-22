using AutoMapper;
using Core.Entities;
using Core.Enum;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
using Services.Services.BusService.DTO;
using Services.Services.TripService.DTO;
using Services.Services.Tripv2Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Tripv2Service
{
	public class Tripv2Service : ITripv2Service
	{
		private readonly IUnitOfWorkv2 _unitOfWork;
		private readonly IMapper _mapper;

        public Tripv2Service(IUnitOfWorkv2 unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Tripv2Dto>> GetAllTripsAsync()
        {
            var trips = await _unitOfWork.Repository<Trips>()
                .GetAllIncludingAsync(t => t.BusTrips);
            return _mapper.Map<IEnumerable<Tripv2Dto>>(trips);
        }

        public async Task<Tripv2Dto> GetTripByIdAsync(int tripId)
        {
            var trip = await _unitOfWork.Repository<Trips>()
                .GetFirstOrDefaultIncludingAsync(t => t.Id == tripId, t => t.BusTrips);
            return _mapper.Map<Tripv2Dto>(trip);
        }

        public async Task<IEnumerable<Tripv2Dto>> GetTripsByBusIdAsync(int busId)
        {
            var trips = await _unitOfWork.Repository<Trips>()
                .FindAllIncludingAsync(
                    t => t.BusTrips.Any(bt => bt.BusId == busId),
                    t => t.BusTrips);
            return _mapper.Map<IEnumerable<Tripv2Dto>>(trips);
        }

        public async Task<IEnumerable<Tripv2Dto>> GetTripsByRouteAsync(string route)
        {
            var trips = await _unitOfWork.Repository<Trips>()
                .FindAllIncludingAsync(
                    t => t.Route.Origin == route || t.Route.Destination == route,
                    t => t.BusTrips);
            return _mapper.Map<IEnumerable<Tripv2Dto>>(trips);
        }
        public async Task<DailySchedule> GetDailyScheduleAsync(RouteScheduleFilter filter)
        {
            // Set default date to today if not provided
            var date = filter.Date ?? DateOnly.FromDateTime(DateTime.Now);

            // Get routes with their trips
            var routes = await _unitOfWork.Repository<Route>()
                .GetAllIncludingAsync(r => r.Trips);

            // Filter routes based on origin and destination
            var filteredRoutes = routes
                .Where(r =>
                    (!string.IsNullOrEmpty(filter.Origin) ? r.Origin == filter.Origin : true) &&
                    (!string.IsNullOrEmpty(filter.Destination) ? r.Destination == filter.Destination : true))
                .Select(r => new RouteScheduleDto
                {
                    RouteId = r.Id,
                    Origin = r.Origin,
                    Destination = r.Destination,
                    TripsToday = r.Trips
                        .Where(t => DateOnly.FromDateTime(t.StartTime) == date)
                        .OrderBy(t => t.StartTime)
                        .Select(t =>
                        {
                            var tripDto = new Schedule
                            {
                                TripId = t.Id,
                                StartTime = t.StartTime
                            };
                            return tripDto;
                        })
                        .ToList()
                })
                .Where(r => r.TripsToday.Any()) // Only include routes with trips today
                .ToList();

            // If direction filter is specified, filter trips
            if (filter.IsOutbound.HasValue)
            {
                filteredRoutes = filteredRoutes
                    .Select(r => new RouteScheduleDto
                    {
                        RouteId = r.RouteId,
                        Origin = r.Origin,
                        Destination = r.Destination,
                        TripsToday = r.TripsToday
                            .ToList()
                    })
                    .Where(r => r.TripsToday.Any())
                    .ToList();
            }

            return new DailySchedule
            {
                Routes = filteredRoutes
            };
        }
        public async Task<TripOperationDto> CreateTripAsync(CRUDTripDto dto)
        {
            var trip = _mapper.Map<Trips>(dto);
            _unitOfWork.Repository<Trips>().Add(trip);
            await _unitOfWork.Complete();
            return _mapper.Map<TripOperationDto>(trip);
        }

        public async Task<bool> UpdateTripAsync(int id, CRUDTripDto dto)
        {
            var trip = await _unitOfWork.Repository<Trips>().GetByIdAsync(id);
            if (trip == null || trip.IsDeleted) return false;

            _mapper.Map(dto, trip); // Update fields
            _unitOfWork.Repository<Trips>().Update(trip);
            await _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            var trip = await _unitOfWork.Repository<Trips>().GetByIdAsync(id);
            if (trip == null) return false;

            trip.IsDeleted = true;
            await _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> SetTripStatusAsync(int tripId, TripStatus status)
        {
            var trip = await _unitOfWork.Repository<Trips>().GetByIdAsync(tripId);
            if (trip == null || trip.IsDeleted) return false;

            trip.Status = status;
            await _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> AssignDriverToTripAsync(int tripId, int driverId)
        {
            var tripRepo = _unitOfWork.Repository<Trips>();
            var driverRepo = _unitOfWork.Repository<Driver>();

            var trip = await tripRepo.GetByIdAsync(tripId);
            var driver = await driverRepo.GetByIdAsync(driverId);
            if (trip == null || driver == null || trip.IsDeleted) return false;

            var isDriverBusy = (await tripRepo.FindAllAsync(t => t.DriverId == driverId && !t.IsDeleted && t.Status == TripStatus.Online)).Any();
            if (isDriverBusy) return false;

            trip.DriverId = driverId;
            driver.Status = DriverStatus.Driving;

            await _unitOfWork.Complete();
            return true;
        }
        public async Task<bool> UnassignDriverFromTripAsync(int tripId)
        {
            var tripRepo = _unitOfWork.Repository<Trips>();
            var driverRepo = _unitOfWork.Repository<Driver>();

            var trip = await tripRepo.GetByIdAsync(tripId);
            if (trip == null || !trip.DriverId.HasValue) return false;

            var driver = await driverRepo.GetByIdAsync(trip.DriverId.Value);
            trip.DriverId = null;
            driver.Status = DriverStatus.Available;

            await _unitOfWork.Complete();
            return true;
        }
    }
}
