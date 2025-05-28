using Services.Services.Tripv2Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.Services.Tripv2Service
{
	public interface ITripv2Service
	{
		Task<IEnumerable<Tripv2Dto>> GetAllTripsAsync();
		Task<Tripv2Dto> GetTripByIdAsync(int tripId);
		// Get trips for a specific bus
		Task<IEnumerable<Tripv2Dto>> GetTripsByBusIdAsync(int busId);

		// Get trips for a specific route
		Task<IEnumerable<Tripv2Dto>> GetTripsByRouteAsync(string route);
        Task<DailySchedule> GetDailyScheduleAsync(RouteScheduleFilter filter);
    }
}
