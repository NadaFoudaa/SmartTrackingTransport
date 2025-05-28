using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Tripv2Service.DTO
{
    public class Schedule
    {
        public int TripId { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class RouteScheduleFilter
    {
        public DateOnly? Date { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public bool? IsOutbound { get; set; }
    }

    public class RouteScheduleDto
    {
        public int RouteId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public List<Schedule> TripsToday { get; set; }
    }

    public class DailySchedule
    {
        public List<RouteScheduleDto> Routes { get; set; }
    }
}
