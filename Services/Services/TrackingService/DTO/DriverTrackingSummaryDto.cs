using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TrackingService.DTO
{
    public class DriverTrackingSummaryDto
    {
        public string DriverName { get; set; }
        public string PhoneNumber { get; set; }

        public string BusLicensePlate { get; set; }
        public string BusModel { get; set; }

        public string RouteOrigin { get; set; }
        public string RouteDestination { get; set; }

        public decimal CurrentLatitude { get; set; }
        public decimal CurrentLongitude { get; set; }
        public DateTime? LastTrackedAt { get; set; }
    }
}
