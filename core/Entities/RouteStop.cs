using Infrastucture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class RouteStop : BaseEntity
	{
		public int RouteId { get; set; }
		public Route Route { get; set; }

		public int StopId { get; set; }
		public Stops Stop { get; set; }
		public int Order { get; set; }  // Determines the stop sequence in the route

	}
}
