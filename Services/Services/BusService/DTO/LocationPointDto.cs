using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BusService.DTO
{
	public class LocationPointDto
	{
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
