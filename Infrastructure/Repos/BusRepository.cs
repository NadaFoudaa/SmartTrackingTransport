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
		public async Task<IEnumerable<Buses>> GetAllBusesWithRouteAsync(string[] includes = null)
		{
			IQueryable<Buses> query = _transportContext.Buses;

			if (includes != null)
			{
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
			}

			return await query.ToListAsync();
		}

		public async Task<IEnumerable<Buses>> GetBusesNearLocationAsync(decimal latitude, decimal longitude, double radiusInKm = 5)
		{
			var buses = await _transportContext.Buses
				.Include(b => b.Route)
				.ThenInclude(r=> r.RouteStops)
				.ThenInclude(rs=> rs.Stop)
				.Where(b => b.Status == "Active")
				.ToListAsync();

			// Filter buses within the specified radius
			var nearbyBuses = buses.Where(bus =>
			{
				var distance = CalculateDistance(
					(double)latitude, (double)longitude,
					(double)bus.CurrentLatitude, (double)bus.CurrentLongitude);
				return distance <= radiusInKm;
			}).ToList();

			return nearbyBuses;
		}

		private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
		{
			const double R = 6371; // Earth's radius in kilometers
			var dLat = ToRadians(lat2 - lat1);
			var dLon = ToRadians(lon2 - lon1);
			var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
					Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
					Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
			var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			return R * c;
		}

		private double ToRadians(double degrees)
		{
			return degrees * Math.PI / 180;
		}
	}
}
