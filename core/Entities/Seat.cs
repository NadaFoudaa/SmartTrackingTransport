using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Entities
{
	public class Seat : BaseEntity
	{
		public int TripId { get; set; }
		public string SeatNumber { get; set; }
		public bool IsReserved { get; set; }
		public Trip Trip { get; set; }
		public int? UserId { get; set; }
		public User User { get; set; }
	}
}
