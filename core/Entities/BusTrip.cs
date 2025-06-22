using Infrastucture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BusTrip
    {
        public int BusId { get; set; }
        public Bus Bus { get; set; }

        public int TripsId { get; set; }
        public Trips Trip { get; set; }
    }
}
