using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastucture.DbContexts;
using Infrastucture.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Services.BusService;
using Services.Services.BusService.DTO;
using Services.Services.Busv2Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Busv2Service
{
    public class Busv2Service : IBusv2Service
    {
        private readonly IUnitOfWorkv2 _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TransportContext context;

        public Busv2Service(IUnitOfWorkv2 unitOfWork, IMapper mapper, TransportContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.context = context;
        }
        public async Task<bool> AddBusAsync(Busv2Dto busv2Dto)
        {
            busv2Dto.LicensePlate = GenerateLicensePlate();
            var bus = _mapper.Map<Bus>(busv2Dto);
            await _unitOfWork.Repository<Bus>().Add(bus);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<IEnumerable<Busv2Dto>> GetAll()
        {
            var buses = await context.Bus
                    .Include(b => b.Route)
                        .ThenInclude(r => r.RouteStops)
                            .ThenInclude(rs => rs.Stop)
                    .Include(b => b.TrackingData)
                    .ToListAsync();
            return _mapper.Map<IEnumerable<Busv2Dto>>(buses);
        }
        public async Task<Busv2AbstractDto> GetBusAbstractAsync(int busId)
        {
            var bus = await _unitOfWork.Busv2Repository
                .GetBusByIdWithRouteAsync(b => b.Id == busId,
                    includes: new[] { "Route.RouteStops.Stop" });

            if (bus == null)
                return null;

            return _mapper.Map<Busv2AbstractDto>(bus);

        }

        // Still not implemented
        public async Task<IEnumerable<Busv2Dto>> GetAvailableBusesAsync(string origin, string destination)
        {
            var buses = await _unitOfWork.Busv2Repository.GetAvailableBusesAsync(origin, destination);
            return _mapper.Map<IEnumerable<Busv2Dto>>(buses);
        }

        public async Task<Busv2Dto> GetBusByIdAsync(int id)
        {
            var bus = await _unitOfWork.Repository<Bus>().GetByIdAsync(id);
            return _mapper.Map<Busv2Dto>(bus);
        }

        public async Task<bool> RemoveBusAsync(int id)
        {
            var bus = await _unitOfWork.Repository<Bus>().GetByIdAsync(id);
            if (bus == null)
                return false;
            _unitOfWork.Repository<Bus>().Delete(bus);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> UpdateBusAsync(int id, Busv2Dto busv2Dto)
        {
            var bus = await _unitOfWork.Repository<Bus>().GetByIdAsync(id);
            if (bus == null) return false;

            _mapper.Map(busv2Dto, bus); // AutoMapper updates the entity
            _unitOfWork.Repository<Bus>().Update(bus);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> UpdateBusStatusAsync(int id, string status)
        {
            var bus = await _unitOfWork.Repository<Bus>().GetByIdAsync(id);
            if (bus == null) return false;

            bus.Status = status;
            _unitOfWork.Repository<Bus>().Update(bus);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<Busv2TripDetailsDto> GetBusTripDetailsAsync(int busId)
        {
            var bus = await _unitOfWork.Busv2Repository
                .GetQueryable()
                .Where(b => b.Id == busId)
                .Include(b => b.Route)
                    .ThenInclude(r => r.RouteStops)
                        .ThenInclude(rs => rs.Stop)
                .Include(b => b.BusTrips)
                    .ThenInclude(bt => bt.Trip)
                .Include(b => b.TrackingData)
                .FirstOrDefaultAsync();

            if (bus == null)
                return null;

            return _mapper.Map<Busv2TripDetailsDto>(bus);
        }
        public async Task<Busv2TripsDto> GetBusTripsFromOriginAsync(string busNumber, string origin, DateTime date)
        {
            var bus = await _unitOfWork.Busv2Repository
                .GetFirstOrDefaultIncludingAsync(
                    b => b.LicensePlate == busNumber,
                    b => b.Route.RouteStops.Select(rs => rs.Stop),
                    b => b.BusTrips.Select(bt => bt.Trip.Route.RouteStops.Select(rs => rs.Stop))
                );

            if (bus == null)
                return null;

            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            // Filter trips for the specified date and origin
            var filteredTrips = bus.BusTrips?
                .Select(bt => bt.Trip)
                .Where(t =>
                    t != null &&
                    t.StartTime >= startOfDay &&
                    t.StartTime < endOfDay &&
                    t.Route?.RouteStops?.Any(rs => rs.Stop?.Name == origin) == true
                )
                .OrderBy(t => t.StartTime)
                .ToList() ?? new List<Trips>();

            // Return a DTO with empty trips list instead of null
            return new Busv2TripsDto
            {
                BusNumber = bus.LicensePlate,
                Origin = origin,
                TripsToday = filteredTrips.Select(t => new TripTimev2Dto
                {
                    TripId = t.Id,
                    StartTime = t.StartTime
                }).ToList()
            };
        }

        public async Task<Busv2TripsDto> GetBusTripsToDestinationAsync(string busNumber, string destination, DateTime date)
        {
            var bus = await _unitOfWork.Busv2Repository
                 .GetFirstOrDefaultIncludingAsync(
                     b => b.LicensePlate == busNumber,
                     b => b.Route.RouteStops.Select(rs => rs.Stop),
                     b => b.BusTrips.Select(bt => bt.Trip.Route.RouteStops.Select(rs => rs.Stop))
                 );

            if (bus == null)
                return null;

            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            // Filter trips for the specified date and destination
            var filteredTrips = bus.BusTrips?
                .Select(bt => bt.Trip)
                .Where(t => t != null
                    && t.StartTime >= startOfDay
                    && t.StartTime < endOfDay
                    && t.Route?.RouteStops?.Any(rs => rs.Stop?.Name == destination) == true)
                .OrderBy(t => t.StartTime)
                .ToList() ?? new List<Trips>();

            // Return a DTO with empty trips list instead of null
            return new Busv2TripsDto
            {
                BusNumber = bus.LicensePlate,
                Destination = destination,
                TripsToday = filteredTrips.Select(t => new TripTimev2Dto
                {
                    TripId = t.Id,
                    StartTime = t.StartTime
                }).ToList()
            };
        }
        static string GenerateLicensePlate()
        {
            var random = new Random();
            var builder = new StringBuilder();

            // Generate 3 random uppercase letters
            for (int i = 0; i < 3; i++)
            {
                char letter = (char)random.Next('A', 'Z' + 1);
                builder.Append(letter);
            }

            builder.Append("-");

            // Generate 4 random digits
            for (int i = 0; i < 4; i++)
            {
                int digit = random.Next(0, 10);
                builder.Append(digit);
            }

            return builder.ToString();
        }
        public async Task<bool> AssignDriverAsync(AssignDriverDto dto)
        {
            var bus = await _unitOfWork.Repository<Bus>().GetByIdAsync(dto.BusId);
            if (bus == null) return false;

            var driver = await _unitOfWork.Repository<Driver>().GetByIdAsync(dto.DriverId);
            if (driver == null) return false;

            var isDriverAssigned = await context.Bus
                .AnyAsync(b => b.DriverId == dto.DriverId && b.Id != dto.BusId);
            if (isDriverAssigned) return false;

            bus.DriverId = dto.DriverId;
            _unitOfWork.Repository<Bus>().Update(bus);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<bool> UnassignDriverAsync(UnassignDriverDto dto)
        {
            var bus = await _unitOfWork.Repository<Bus>().GetByIdAsync(dto.BusId);
            if (bus == null || bus.DriverId == null) return false;

            bus.DriverId = null;
            _unitOfWork.Repository<Bus>().Update(bus);
            return await _unitOfWork.Complete() > 0;
        }
    }
}