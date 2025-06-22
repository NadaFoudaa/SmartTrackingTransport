using Infrastucture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
	public interface IBusRepository : IGenericRepository<Buses>
	{
		Task<IEnumerable<Buses>> GetAvailableBusesAsync(string startLocation, string destination);
		Task<Buses> GetBusByIdWithRouteAsync(Expression<Func<Buses, bool>> predicate,string[] includes);
		Task<IEnumerable<Buses>> GetAllBusesWithRouteAsync(string[] includes = null);
		//Task<IEnumerable<Buses>> GetBusesNearLocationAsync(decimal latitude, decimal longitude, double radiusInKm = 5.0);


	}
}