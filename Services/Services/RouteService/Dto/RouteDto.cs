using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.RouteService.Dto
{
	public class RouteDto
	{
		public int RouteId { get; set; }
		public string Origin { get; set; }
		public string Destination { get; set; }
		public List<string> Stops { get; set; }
	}
}
