using Services.Services.BusService.DTO;
using Services.Services.Busv2Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Busv2Service
{
	public interface IBusv2Service
	{
		Task<IEnumerable<Busv2Dto>> GetAll();
		Task<Busv2AbstractDto> GetBusAbstractAsync(int busId);
		Task<Busv2TripDetailsDto> GetBusTripDetailsAsync(int busId);
		Task<Busv2TripsDto> GetBusTripsFromOriginAsync(string busNumber, string origin, DateTime date);
		Task<Busv2TripsDto> GetBusTripsToDestinationAsync(string busNumber, string destination, DateTime date);
		Task<IEnumerable<Busv2Dto>> GetAvailableBusesAsync(string origin, string destination);
		Task<bool> AddBusAsync(Busv2Dto busv2Dto);
		Task<bool> UpdateBusAsync(int id, Busv2Dto busv2Dto);
		Task<bool> RemoveBusAsync(int id);
		Task<Busv2Dto> GetBusByIdAsync(int id);
		Task<bool> UpdateBusStatusAsync(int id, string status);

	}
}