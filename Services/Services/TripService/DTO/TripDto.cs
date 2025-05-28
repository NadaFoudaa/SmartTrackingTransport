using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TripService.DTO
{
	public class TripDto
	{
		public int TripId { get; set; }
		public int BusId { get; set; }
		public int RouteId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }
	}
}
