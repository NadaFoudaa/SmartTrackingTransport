using Services.Services.BusService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BusService
{
	public interface IBusService
	{
		Task<IEnumerable<BusDto>> GetAll();
		Task<BusAbstractDto> GetBusAbstractAsync(int busId);
		Task<BusTripDetailsDto> GetBusTripDetailsAsync(int busId);
		Task<BusTripsDto> GetBusTripsFromOriginAsync(string busNumber, string origin, DateTime date);
		Task<BusTripsDto> GetBusTripsToDestinationAsync(string busNumber, string destination, DateTime date);
		Task<IEnumerable<BusDto>> GetAvailableBusesAsync(string origin, string destination);
		Task<bool> AddBusAsync(BusDto busDto);
		Task<bool> UpdateBusAsync(int id, BusDto busDto);
		Task<bool> RemoveBusAsync(int id);
		Task<BusDto> GetBusByIdAsync(int id);
		Task<bool> UpdateBusStatusAsync(int id, string status);
		//Task<IEnumerable<BusDto>> GetBusesNearLocationAsync(decimal latitude, decimal longitude, double radiusInKm = 5.0);


	}
}