using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Entities
{
	public class Buses : BaseEntity
	{
		public string LicensePlate { get; set; }
		public string Model { get; set; }
		public int Capacity { get; set; }
		public string Status { get; set; }
		public decimal CurrentLatitude { get; set; } // Last known location
		public decimal CurrentLongitude { get; set; }
		public int? DriverId { get; set; }
		public Driver Driver { get; set; }
		public int? RouteId { get; set; }
		public Route Route { get; set; }
		public ICollection<Trip> Trips { get; set; }

		//well be deleted if we use websocket
		public ICollection<TrackingData> TrackingData { get; set; }
	}
}
