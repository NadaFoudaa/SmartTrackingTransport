using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Entities
{
	public class Trip : BaseEntity
	{
		public int BusId { get; set; }
		public int RouteId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public Buses Bus { get; set; }
		public Route Route { get; set; }
	}
}
