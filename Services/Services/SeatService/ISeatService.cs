using Services.Services.SeatService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.SeatService
{
    public interface ISeatService
    {
        Task<TripSeatLayoutDto> GetSeatLayoutAsync(int tripId, int userId);
        Task<string> ReserveSeatAsync(ReserveSeatDto dto, int userId);
    }
}
