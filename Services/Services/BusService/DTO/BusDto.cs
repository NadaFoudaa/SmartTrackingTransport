using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BusService.DTO
{
	public class BusDto
	{
		public int Id { get; set; }
		public string LicensePlate { get; set; }
		public string Model { get; set; }
		public int Capacity { get; set; }
		public string Status { get; set; }
		public string Origin { get; set; }
		public string Destination { get; set; }
	}
}
