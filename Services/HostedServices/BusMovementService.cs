/*
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services.HostedServices
{
	/// <summary>
	/// Holds the real-time state of a bus's journey for the simulation.
	/// </summary>
	public class BusState
	{
		public int CurrentStopIndex { get; set; } = 0;
		public double ProgressToNextStop { get; set; } = 0.0; // From 0.0 to 1.0
	}

	/// <summary>
	/// A background service that simulates the movement of buses along their routes.
	/// </summary>
	public class BusMovementService : IHostedService, IDisposable
	{
		private readonly ILogger<BusMovementService> _logger;
		private readonly IServiceScopeFactory _scopeFactory;
		private Timer _timer;
		private readonly ConcurrentDictionary<int, BusState> _busStates = new ConcurrentDictionary<int, BusState>();

		// Simulation settings
		private const double BusSpeedKmph = 40.0; // Speed of the bus in km/h
		private const int UpdateIntervalSeconds = 2; // How often to update bus positions

		public BusMovementService(ILogger<BusMovementService> logger, IServiceScopeFactory scopeFactory)
		{
			_logger = logger;
			_scopeFactory = scopeFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Bus Movement Simulation Service is starting.");
			_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(UpdateIntervalSeconds));
			return Task.CompletedTask;
		}

		private async void DoWork(object state)
		{
			_logger.LogInformation("Bus Movement Simulation Service is running.");

			using (var scope = _scopeFactory.CreateScope())
			{
				var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

				var buses = await unitOfWork.BusRepository.GetAllBusesWithRouteAsync(new[] { "Route.RouteStops.Stop" });
				var activeBuses = buses.Where(b => b.Status == "Active" && b.Route?.RouteStops?.Any() == true).ToList();

				foreach (var bus in activeBuses)
				{
					var busState = _busStates.GetOrAdd(bus.Id, new BusState());
					var orderedStops = bus.Route.RouteStops.OrderBy(rs => rs.Order).ToList();

					if (orderedStops.Count < 2) continue;

					// Ensure the bus loops back to the start
					if (busState.CurrentStopIndex >= orderedStops.Count - 1)
					{
						busState.CurrentStopIndex = 0;
						busState.ProgressToNextStop = 0;
					}

					var currentStop = orderedStops[busState.CurrentStopIndex].Stop;
					var nextStop = orderedStops[busState.CurrentStopIndex + 1].Stop;

					// Calculate distance and update progress
					var distanceToNextStop = CalculateDistance(
						(double)currentStop.Latitude, (double)currentStop.Longitude,
						(double)nextStop.Latitude, (double)nextStop.Longitude);

					if (distanceToNextStop > 0)
					{
						var speedMetersPerSecond = (BusSpeedKmph * 1000) / 3600;
						var distanceToTravel = speedMetersPerSecond * UpdateIntervalSeconds;
						busState.ProgressToNextStop += (distanceToTravel / (distanceToNextStop * 1000));
					}

					if (busState.ProgressToNextStop >= 1.0)
					{
						busState.ProgressToNextStop = 0;
						busState.CurrentStopIndex++;
						// Re-check for loop
						if (busState.CurrentStopIndex >= orderedStops.Count - 1)
						{
							busState.CurrentStopIndex = 0;
						}
					}

					// Interpolate position
					var newPosition = Interpolate(currentStop, nextStop, busState.ProgressToNextStop);
					bus.CurrentLatitude = (decimal)newPosition.Lat;
					bus.CurrentLongitude = (decimal)newPosition.Lon;

					unitOfWork.Repository<Buses>().Update(bus);
				}

				await unitOfWork.Complete();
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Bus Movement Simulation Service is stopping.");
			_timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}

		// --- Helper Methods ---

		private (double Lat, double Lon) Interpolate(Stops from, Stops to, double progress)
		{
			var lat = (double)from.Latitude + ((double)to.Latitude - (double)from.Latitude) * progress;
			var lon = (double)from.Longitude + ((double)to.Longitude - (double)from.Longitude) * progress;
			return (lat, lon);
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
*/