using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.ReservationService
{
	using global::Services.Services.ReservationService.DTO;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	namespace Infrastructure.Interfaces
	{
		public interface IReservationService
		{
			// Fetch all reservations (returns SeatReservationDto, as they represent confirmed reservations)
			Task<IEnumerable<SeatReservationDto>> GetAllReservationsAsync();

			// Fetch a specific reservation by ID (returns SeatReservationDto)
			Task<SeatReservationDto> GetReservationByIdAsync(int id);

			// Add a new reservation (taking BookSeatDto, and returning a SeatReservationDto after booking)
			Task<SeatReservationDto> AddReservationAsync(BookSeatDto bookSeatDto);

			// Update an existing reservation (using SeatReservationDto to update)
			Task<bool> UpdateReservationAsync(int id, SeatReservationDto seatReservationDto);

			// Cancel a reservation (returns a boolean indicating success)
			Task<bool> CancelReservationAsync(int id);
		}
	}

}
