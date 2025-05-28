using Services.Services.StopsService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.StopsService
{
	public interface IStopsService
	{
		Task<IEnumerable<StopsDto>> GetAllStopsForRouteAsync(int routeId);
		Task<StopsDto> GetStopByIdAsync(int stopId);

	}
}
