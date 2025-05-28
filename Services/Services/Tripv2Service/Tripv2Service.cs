using AutoMapper;
using Core.Entities;
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
    }
}
