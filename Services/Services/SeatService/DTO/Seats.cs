using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.SeatService.DTO
{
    public class TripSeatLayoutDto
    {
        public int TripId { get; set; }
        public DateTime StartTime { get; set; }
        public BusInfoDto Bus { get; set; }
        public List<SeatDto> Seats { get; set; }
    }

    public class BusInfoDto
    {
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public int Capacity { get; set; }
    }

    public class SeatDto
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public bool IsReserved { get; set; }
        public bool IsMine { get; set; }
    }
    public class ReserveSeatDto
    {
        public int TripId { get; set; }
        public string SeatNumber { get; set; }
    }
}
