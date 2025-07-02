using Services.Services.TrackingService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TrackingService
{
    public interface ITrackingService
    {
        Task<BusTrackingLiteDto> QuickUpdateBusLocationAsync(TrackingDataDto dto, DateTime timestamp);
        Task<BusTrackingUpdateDto> GetBusCurrentTrackingAsync(int busId);
        Task<LocationDto> GetBusLocationOnlyAsync(int busId);
        Task<List<NearbyBusDto>> GetNearbyBusesAsync(decimal userLat, decimal userLon, double radiusMeters);
    }
}
