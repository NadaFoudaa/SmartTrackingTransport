using AutoMapper;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
using Services.Services.TripService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TripService
{
	public class TripService : ITripService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public TripService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<TripDto>> GetAllTripsAsync()
		{
			var trips = await _unitOfWork.Repository<Trip>().GetAllAsync();
			return _mapper.Map<IEnumerable<TripDto>>(trips);
		}

		public async Task<TripDto> GetTripByIdAsync(int tripId)
		{
			var trip = await _unitOfWork.Repository<Trip>().GetByIdAsync(tripId);
			return _mapper.Map<TripDto>(trip);
		}
		public async Task<IEnumerable<TripDto>> GetTripsByBusIdAsync(int busId)
		{
			var trips = await _unitOfWork.Repository<Trip>()
				.FindAllAsync(t => t.BusId == busId); // Filter trips by BusId

			return _mapper.Map<IEnumerable<TripDto>>(trips); // Map to TripDto
		}

		// Get trips for a specific route
		public async Task<IEnumerable<TripDto>> GetTripsByRouteAsync(string route)
		{
			var trips = await _unitOfWork.Repository<Trip>()
				.FindAllAsync(t => t.Route.Origin == route || t.Route.Destination == route); // Filter trips by Route

			return _mapper.Map<IEnumerable<TripDto>>(trips); // Map to TripDto
		}
	}
}
