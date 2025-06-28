using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Services.Services.StopsService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.StopsService
{
	public class StopsService : IStopsService
	{
		private readonly IUnitOfWorkv2 _unitOfWork;
		private readonly IMapper _mapper;
		public StopsService(IUnitOfWorkv2 unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<IEnumerable<StopsDto>> GetAllStopsForRouteAsync(int routeId)
		{
			var stops = await _unitOfWork.Repository<RouteStop>().FindAllAsync(rs => rs.RouteId == routeId);
			return _mapper.Map<IEnumerable<StopsDto>>(stops);
		}

		// Get a stop by its ID
		public async Task<StopsDto> GetStopByIdAsync(int stopId)
		{
			var stop = await _unitOfWork.Repository<Stops>().GetByIdAsync(stopId);
			return stop == null ? null : _mapper.Map<StopsDto>(stop);
		}
	}
}
