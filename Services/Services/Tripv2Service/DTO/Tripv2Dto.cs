using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Tripv2Service.DTO
{
	public class Tripv2Dto
	{
		public int TripId { get; set; }
        public List<int> BusIds { get; set; } = new List<int>();

        public int RouteId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }
	}
}
