using Infrastucture.DbContexts;
using Infrastucture.Entities;
using Services.Services.TrackingService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Helper;
using Core.Entities;

namespace Services.Services.TrackingService
{
    public class TrackingService
    {
        private readonly TransportContext _context;

        public TrackingService(TransportContext context)
        {
            _context = context;
        }

        public async Task<BusTrackingUpdateDto> ProcessTrackingAsync(TrackingDataDto dto)
        {
            var tracking = new TrackingData
            {
                BusId = dto.BusId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Timestamp = DateTime.UtcNow
            };

            _context.TrackingData.Add(tracking);
            await _context.SaveChangesAsync();

            var bus = await _context.Bus
                .Include(b => b.Route)
                    .ThenInclude(r => r.RouteStops)
                        .ThenInclude(rs => rs.Stop)
                .FirstOrDefaultAsync(b => b.Id == dto.BusId);

            if (bus?.Route == null)
                throw new Exception("Bus not assigned to a route");

            var orderedStops = bus.Route.RouteStops
                .OrderBy(rs => rs.Order)
                .Select(rs => rs.Stop)
                .ToList();

            Stops nextStop = null;
            double minDistance = double.MaxValue;

            foreach (var stop in orderedStops)
            {
                double dist = GeoUtils.Haversine(
                    (double)dto.Latitude, (double)dto.Longitude,
                    (double)stop.Latitude, (double)stop.Longitude);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    nextStop = stop;
                }
            }

            const double avgSpeedKmph = 40;
            double timeHours = (minDistance / 1000.0) / avgSpeedKmph;
            int etaMinutes = (int)Math.Round(timeHours * 60);

            return new BusTrackingUpdateDto
            {
                BusId = dto.BusId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                NextStopName = nextStop?.Name ?? "Unknown",
                DistanceToNextStopMeters = Math.Round(minDistance),
                EstimatedTimeMinutes = etaMinutes
            };
        }
    }
}
