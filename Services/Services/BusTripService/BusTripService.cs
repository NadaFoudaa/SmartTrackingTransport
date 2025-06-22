using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Services.Services.BusTripService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BusTripService
{
    public class BusTripService : IBusTripService
    {
        private readonly IUnitOfWorkv2 _unitOfWork;
        private readonly IMapper _mapper;

        public BusTripService(IUnitOfWorkv2 unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BusTripDto>> GetAllAsync()
        {
            var busTrips = await _unitOfWork.Repository<BusTrip>().GetAllAsync();
            return _mapper.Map<IEnumerable<BusTripDto>>(busTrips);
        }

        public async Task<BusTripDto> GetByIdsAsync(int busId, int tripId)
        {
            var busTrip = await _unitOfWork.Repository<BusTrip>()
                .FindAllAsync(bt => bt.BusId == busId && bt.TripsId == tripId);
            return _mapper.Map<BusTripDto>(busTrip.FirstOrDefault());
        }

        public async Task AddAsync(BusTripDto dto)
        {
            var entity = _mapper.Map<BusTrip>(dto);
            await _unitOfWork.Repository<BusTrip>().Add(entity);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(int busId, int tripId)
        {
            var busTrip = (await _unitOfWork.Repository<BusTrip>()
                .FindAllAsync(bt => bt.BusId == busId && bt.TripsId == tripId)).FirstOrDefault();

            if (busTrip != null)
            {
                _unitOfWork.Repository<BusTrip>().Delete(busTrip);
                await _unitOfWork.Complete();
            }
        }
    }
}