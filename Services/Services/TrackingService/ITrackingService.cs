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
        Task<BusTrackingUpdateDto> ProcessTrackingAsync(TrackingDataDto dto);
        Task<List<DriverTrackingSummaryDto>> GetAllDriverTrackingSummariesAsync();
    }
}
