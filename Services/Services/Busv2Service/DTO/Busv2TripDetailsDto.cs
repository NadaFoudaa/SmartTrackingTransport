using Services.Services.BusService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Busv2Service.DTO
{
	public class Busv2TripDetailsDto
	{
		public string BusNumber { get; set; }
		public string Origin { get; set; }
		public string Destination { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public List<StopTimev2Dto> Stops { get; set; }
		public List<LocationPointv2Dto> LifeTrack { get; set; }
	}
}
