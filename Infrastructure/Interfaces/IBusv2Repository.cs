using Core.Entities;
using Infrastucture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
	public interface IBusv2Repository : IGenericRepository<Bus>
	{
		Task<IEnumerable<Bus>> GetAvailableBusesAsync(string startLocation, string destination);
		Task<Bus> GetBusByIdWithRouteAsync(Expression<Func<Bus, bool>> predicate,string[] includes);

	}
}