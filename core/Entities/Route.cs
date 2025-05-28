using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Entities
{
	public class Route : BaseEntity
	{
		public string Origin { get; set; }
		public string Destination { get; set; }
		public ICollection<RouteStop> RouteStops { get; set; } = new List<RouteStop>();
		public ICollection<Trip> Trips { get; set; }
		public ICollection<Buses> Buses { get; set; }

	}
}
