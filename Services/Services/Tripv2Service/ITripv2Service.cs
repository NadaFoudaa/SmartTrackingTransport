using Core.Entities;
using Core.Enum;
using Infrastucture.Entities;
using Services.Services.TripService.DTO;
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
        // Trip CRUD
        Task<TripOperationDto> CreateTripAsync(CRUDTripDto dto);
        Task<bool> UpdateTripAsync(int tripId, CRUDTripDto dto);
        Task<bool> DeleteTripAsync(int tripId);

        // Trip Status
        Task<bool> SetTripStatusAsync(int tripId, TripStatus status);

        // Driver Assignment
        Task<bool> AssignDriverToTripAsync(int tripId, int driverId);
        Task<bool> UnassignDriverFromTripAsync(int tripId);
    }
}
