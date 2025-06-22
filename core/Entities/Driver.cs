using Core.Entities;
using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Entities
{
    public class Driver : BaseEntity
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string LicenseNumber { get; set; }
        public Buses Bus { get; set; }
        public Bus Buses { get; set; }
        public DriverStatus Status { get; set; }
        public ICollection<Trips> Trip { get; set; }

    }
}
