using Infrastucture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ICollection<BusTrip> BusTrips { get; set; }
    }
}
