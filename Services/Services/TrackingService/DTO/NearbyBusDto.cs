using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TrackingService.DTO
{
    public class NearbyBusDto
    {
        public int BusId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int? DriverId { get; set; }

        public string Origin { get; set; }        // From Route.Origin
        public string Destination { get; set; }   // From Route.Destination
    }
}
