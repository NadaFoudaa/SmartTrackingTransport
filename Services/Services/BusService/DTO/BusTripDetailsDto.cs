using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BusService.DTO
{
	public class BusTripDetailsDto
	{
		public string BusNumber { get; set; }
		public string Origin { get; set; }
		public string Destination { get; set; }
		public List<string> Stops { get; set; }
	}
}
