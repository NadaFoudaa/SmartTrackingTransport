using Infrastructure.Interfaces;
using Infrastucture.DbContexts;
using Infrastucture.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public class BusRepository : GenericRepository<Buses>, IBusRepository
	{
		public BusRepository(TransportContext transportContext) : base(transportContext)
		{
		}

		public async Task<IEnumerable<Buses>> GetAvailableBusesAsync(string startLocation, string destination)
		{
			var buses = await _transportContext.Buses
				.Include(b => b.Route)
					.ThenInclude(r => r.RouteStops)
						.ThenInclude(rs => rs.Stop)
				.Include(b => b.Driver)
				.Where(b =>
					b.Status == "Active" &&
					b.Route != null &&
					b.Route.RouteStops.Any(rs => rs.Stop.Name.Replace(" ","").Replace("-","").Trim().ToLower() == startLocation.Trim().ToLower()) &&
					b.Route.RouteStops.Any(rs => rs.Stop.Name.Replace(" ","").Replace("-","").Trim().ToLower() == destination.Trim().ToLower()))
				.ToListAsync();

			// Filter in-memory for stop order
			return buses.Where(b =>
			{
				var origin = b.Route.RouteStops.FirstOrDefault(rs => rs.Stop.Name.Replace(" ", "").Replace("-","").Trim().Equals(startLocation, StringComparison.OrdinalIgnoreCase));
				var dest = b.Route.RouteStops.FirstOrDefault(rs => rs.Stop.Name.Replace(" ","").Replace("-","").Trim().Equals(destination, StringComparison.OrdinalIgnoreCase));
				return origin != null && dest != null && origin.Order < dest.Order;
			});
		}


		public async Task<Buses> GetBusByIdWithRouteAsync(
	        Expression<Func<Buses, bool>> predicate,
	        string[] includes = null)
		{
			IQueryable<Buses> query = _transportContext.Buses;

			if (includes != null)
			{
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
			}

			return await query.FirstOrDefaultAsync(predicate);
		}

       
    }
}
