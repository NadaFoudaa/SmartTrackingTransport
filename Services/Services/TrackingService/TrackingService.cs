using AutoMapper;
using Core.Entities;
using Core.Helper;
using Infrastucture.DbContexts;
using Infrastucture.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Services.TrackingService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services.TrackingService
{
    public class TrackingService : ITrackingService
    {
        private readonly TransportContext _context;
        private const double AvgSpeedKmph = 40.0;
        private readonly IMapper _mapper;


        public TrackingService(TransportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<BusTrackingLiteDto> QuickUpdateBusLocationAsync(TrackingDataDto dto, DateTime timestamp)
        {
            var bus = await _context.Bus.FirstOrDefaultAsync(b => b.Id == dto.BusId);
            if (bus == null) throw new Exception("Bus not found");

            bus.CurrentLatitude = dto.Latitude;
            bus.CurrentLongitude = dto.Longitude;
            _context.Bus.Update(bus);

            _context.TrackingData.Add(new TrackingData
            {
                BusId = dto.BusId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Timestamp = timestamp
            });
            Console.WriteLine($" DTO.Latitude: {dto.Latitude}");
            Console.WriteLine($" Tracked Latitude: {_context.Entry(bus).Property(b => b.CurrentLatitude).CurrentValue}");

            await _context.SaveChangesAsync();

            return new BusTrackingLiteDto
            {
                BusId = dto.BusId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
            };
        }
        public async Task<BusTrackingUpdateDto> GetBusCurrentTrackingAsync(int busId)
        {
            var bus = await _context.Bus
                .Include(b => b.Driver) // 👈 Ensure Driver is included
                .Include(b => b.Route)
                    .ThenInclude(r => r.RouteStops)
                        .ThenInclude(rs => rs.Stop)
                .FirstOrDefaultAsync(b => b.Id == busId);

            if (bus == null || bus.Route == null)
                return null;

            var orderedStops = bus.Route.RouteStops
                .OrderBy(rs => rs.Order)
                .Select(rs => rs.Stop)
                .ToList();

            if (orderedStops.Count == 0)
                return null;

            Stops nextStop = null;
            double minDistance = double.MaxValue;

            foreach (var stop in orderedStops)
            {
                double dist = GeoUtils.Haversine(
                    (double)bus.CurrentLatitude,
                    (double)bus.CurrentLongitude,
                    (double)stop.Latitude,
                    (double)stop.Longitude
                );

                if (dist < minDistance)
                {
                    minDistance = dist;
                    nextStop = stop;
                }
            }

            double timeHours = (minDistance / 1000.0) / AvgSpeedKmph;
            int etaMinutes = (int)Math.Round(timeHours * 60);

            return new BusTrackingUpdateDto
            {
                BusId = bus.Id,
                Latitude = bus.CurrentLatitude,
                Longitude = bus.CurrentLongitude,
                NextStopName = nextStop?.Name ?? "Unknown",
                DistanceToNextStopMeters = Math.Round(minDistance),
                EstimatedTimeMinutes = etaMinutes,
                DriverId = bus.Driver?.Id,
                DriverName = bus.Driver?.Name
            };
        }
        public async Task<LocationDto> GetBusLocationOnlyAsync(int busId)
        {
            var bus = await _context.Bus
                .FirstOrDefaultAsync(b => b.Id == busId);

            if (bus == null)
                return null;

            return _mapper.Map<LocationDto>(bus);
        }
        public async Task<List<NearbyBusDto>> GetNearbyBusesAsync(decimal userLat, decimal userLon, double radiusMeters)
        {
            double radiusKm = radiusMeters / 1000.0;
            double lat = (double)userLat;
            double lon = (double)userLon;

            double latDiff = radiusKm / 110.574;
            double lonDiff = radiusKm / (111.320 * Math.Cos(lat * Math.PI / 180));

            var minLat = lat - latDiff;
            var maxLat = lat + latDiff;
            var minLon = lon - lonDiff;
            var maxLon = lon + lonDiff;

            var candidates = await _context.Bus
                .Include(b => b.Route) // Include Route info
                .Where(b => b.CurrentLatitude >= (decimal)minLat && b.CurrentLatitude <= (decimal)maxLat
                         && b.CurrentLongitude >= (decimal)minLon && b.CurrentLongitude <= (decimal)maxLon)
                .ToListAsync();

            var nearbyBuses = candidates
                .Where(b => GeoUtils.Haversine(lat, lon, (double)b.CurrentLatitude, (double)b.CurrentLongitude) <= radiusMeters)
                .Select(b => new NearbyBusDto
                {
                    BusId = b.Id,
                    Latitude = b.CurrentLatitude,
                    Longitude = b.CurrentLongitude,
                    DriverId = b.DriverId,
                    Origin = b.Route?.Origin,
                    Destination = b.Route?.Destination
                })
                .ToList();

            return nearbyBuses;
        }
    }
}