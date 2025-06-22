using Core.Enum;
using Infrastucture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Trips : BaseEntity
    {
        public int RouteId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Route Route { get; set; }
        public int? DriverId { get; set; }       // who is assigned
        public Driver Driver { get; set; }

        public TripStatus Status { get; set; }   // Online, Paused, OutOfService
        public bool IsDeleted { get; set; }

        public ICollection<BusTrip> BusTrips { get; set; }
    }
}
