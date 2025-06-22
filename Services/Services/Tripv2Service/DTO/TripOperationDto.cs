using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Tripv2Service.DTO
{
    public class TripOperationDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int? DriverId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TripStatus Status { get; set; }
        public bool IsDeleted { get; set; }

    }
}
