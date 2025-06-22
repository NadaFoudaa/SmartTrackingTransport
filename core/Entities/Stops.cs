using Infrastucture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class Stops : BaseEntity
	{
		public string Name { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }

		public ICollection<RouteStop> RouteStops { get; set; } = new List<RouteStop>();

	}
}
