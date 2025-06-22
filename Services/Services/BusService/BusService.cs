using AutoMapper;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
using Services.Services.BusService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BusService
{
	public class BusService : IBusService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public BusService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<bool> AddBusAsync(BusDto busDto)
		{
			var bus = _mapper.Map<Buses>(busDto);
			await _unitOfWork.Repository<Buses>().Add(bus);
			return await _unitOfWork.Complete() > 0;
		}

		public async Task<IEnumerable<BusDto>> GetAll()
		{
			var buses = await _unitOfWork.BusRepository.GetAllBusesWithRouteAsync(includes: new[] { "Route.RouteStops.Stop" });
			return _mapper.Map<IEnumerable<BusDto>>(buses);
		}
		public async Task<BusAbstractDto> GetBusAbstractAsync(int busId)
		{
			var bus = await _unitOfWork.BusRepository
				.GetBusByIdWithRouteAsync(b => b.Id == busId,
					includes: new[] { "Route.RouteStops.Stop" });

			if (bus == null)
				return null;

			return _mapper.Map<BusAbstractDto>(bus);

		}

		// Still not implemented
		public async Task<IEnumerable<BusDto>> GetAvailableBusesAsync(string origin, string destination)
		{
			var busRepo = _unitOfWork.Repository<Buses>() as IBusRepository;
			if (busRepo == null)
				throw new InvalidOperationException("Bus repository not found in UnitOfWork");

			var buses = await busRepo.GetAvailableBusesAsync(origin, destination);
			return _mapper.Map<IEnumerable<BusDto>>(buses);
		}

		public async Task<BusDto> GetBusByIdAsync(int id)
		{
			var bus = await _unitOfWork.Repository<Buses>().GetByIdAsync(id);
			return _mapper.Map<BusDto>(bus);
		}

		public async Task<bool> RemoveBusAsync(int id)
		{
			var bus = await _unitOfWork.Repository<Buses>().GetByIdAsync(id);
			if (bus == null)
				return false;
			_unitOfWork.Repository<Buses>().Delete(bus);
			return await _unitOfWork.Complete() > 0;
		}

		public async Task<bool> UpdateBusAsync(int id, BusDto busDto)
		{
			var bus = await _unitOfWork.Repository<Buses>().GetByIdAsync(id);
			if (bus == null) return false;

			_mapper.Map(busDto, bus); // AutoMapper updates the entity
			_unitOfWork.Repository<Buses>().Update(bus);
			return await _unitOfWork.Complete() > 0;
		}

		public async Task<bool> UpdateBusStatusAsync(int id, string status)
		{
			var bus = await _unitOfWork.Repository<Buses>().GetByIdAsync(id);
			if (bus == null) return false;

			bus.Status = status;
			_unitOfWork.Repository<Buses>().Update(bus);
			return await _unitOfWork.Complete() > 0;
		}

		public async Task<BusTripDetailsDto> GetBusTripDetailsAsync(int busId)
		{
			var bus = await _unitOfWork.BusRepository
				.GetBusByIdWithRouteAsync(
					b => b.Id == busId,
					includes: new[] {
						"Route.RouteStops.Stop",
						"Trips",
						"TrackingData"
					}
				);

			if (bus == null)
				return null;

			return _mapper.Map<BusTripDetailsDto>(bus);
		}
		public async Task<BusTripsDto> GetBusTripsFromOriginAsync(string busNumber, string origin, DateTime date)
		{
			var bus = await _unitOfWork.BusRepository
				.GetBusByIdWithRouteAsync(
					b => b.LicensePlate == busNumber,
					includes: new[] {
						"Route.RouteStops.Stop",
						"Trips.Route.RouteStops.Stop"
					}
				);

			if (bus == null)
				return null;

			var startOfDay = date.Date;
			var endOfDay = startOfDay.AddDays(1);

			// Filter trips for the specified date and origin
			var filteredTrips = bus.Trips
				.Where(t => t.StartTime >= startOfDay && t.StartTime < endOfDay)
				.Where(t => t.Route?.RouteStops?.Any(rs => rs.Stop?.Name == origin) ?? false)
				.OrderBy(t => t.StartTime)
				.ToList();

			// Return a DTO with empty trips list instead of null
			return new BusTripsDto
			{
				BusNumber = bus.LicensePlate,
				Origin = origin,
				TripsToday = filteredTrips.Select(t => new TripTimeDto
				{
					TripId = t.Id,
					StartTime = t.StartTime
				}).ToList()
			};
		}

		public async Task<BusTripsDto> GetBusTripsToDestinationAsync(string busNumber, string destination, DateTime date)
		{
			var bus = await _unitOfWork.BusRepository
				.GetBusByIdWithRouteAsync(
					b => b.LicensePlate == busNumber,
					includes: new[] {
						"Route.RouteStops.Stop",
						"Trips.Route.RouteStops.Stop"
					}
				);

			if (bus == null)
				return null;

			var startOfDay = date.Date;
			var endOfDay = startOfDay.AddDays(1);

			// Filter trips for the specified date and destination
			var filteredTrips = bus.Trips
				.Where(t => t.StartTime >= startOfDay && t.StartTime < endOfDay)
				.Where(t => t.Route?.RouteStops?.Any(rs => rs.Stop?.Name == destination) ?? false)
				.OrderBy(t => t.StartTime)
				.ToList();

			// Return a DTO with empty trips list instead of null
			return new BusTripsDto
			{
				BusNumber = bus.LicensePlate,
				Destination = destination,
				TripsToday = filteredTrips.Select(t => new TripTimeDto
				{
					TripId = t.Id,
					StartTime = t.StartTime
				}).ToList()
			};
		}
		/*
		public async Task<IEnumerable<BusDto>> GetBusesNearLocationAsync(decimal latitude, decimal longitude, double radiusInKm = 5)
		{
			var buses = await _unitOfWork.BusRepository.GetBusesNearLocationAsync(latitude, longitude, radiusInKm);
			return _mapper.Map<IEnumerable<BusDto>>(buses);
		}
		*/
	}
}
