using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.ReservationService.DTO
{
	public class BookSeatDto
	{
		public int BusId { get; set; }
		public int TripId { get; set; }
		public int SeatNumber { get; set; }
		public string PassengerName { get; set; }
		public string PassengerPhone { get; set; }
	}
}
