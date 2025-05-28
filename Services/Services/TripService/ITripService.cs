using Services.Services.TripService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TripService
{
	public interface ITripService
	{
		Task<IEnumerable<TripDto>> GetAllTripsAsync();
		Task<TripDto> GetTripByIdAsync(int tripId);
		// Get trips for a specific bus
		Task<IEnumerable<TripDto>> GetTripsByBusIdAsync(int busId);

		// Get trips for a specific route
		Task<IEnumerable<TripDto>> GetTripsByRouteAsync(string route);

	}
}
