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
        public async Task<bool> AssignBusToTripAsync(int tripId, int busId)
        {
            var tripRepo = _unitOfWork.Repository<Trips>();
            var busRepo = _unitOfWork.Repository<Bus>();
            var busTripRepo = _unitOfWork.Repository<BusTrip>();

            var trip = await tripRepo.GetByIdAsync(tripId);
            var bus = await busRepo.GetByIdAsync(busId);

            if (trip == null || bus == null || trip.IsDeleted) return false;

            var alreadyAssigned = (await busTripRepo.FindAllAsync(bt => bt.BusId == busId && bt.TripsId == tripId)).Any();
            if (alreadyAssigned) return false;

            // Link bus to trip
            busTripRepo.Add(new BusTrip
            {
                BusId = busId,
                TripsId = tripId
            });

            // Update the bus's route to reflect the trip's route
            bus.RouteId = trip.RouteId;
            busRepo.Update(bus);

            await _unitOfWork.Complete();
            return true;
        }
        public async Task<bool> UnassignBusFromTripAsync(int tripId, int busId)
        {
            var busTripRepo = _unitOfWork.Repository<BusTrip>();
            var busRepo = _unitOfWork.Repository<Bus>();

            // Find the bus-trip mapping
            var mapping = (await busTripRepo.FindAllAsync(bt => bt.TripsId == tripId && bt.BusId == busId))
                            .FirstOrDefault();

            if (mapping == null) return false;

            // Remove the mapping
            busTripRepo.Delete(mapping);

            // Check if the bus is still used in other trips
            var stillAssigned = (await busTripRepo.FindAllAsync(bt => bt.BusId == busId && bt.TripsId != tripId)).Any();

            if (!stillAssigned)
            {
                // Only clear RouteId if the bus isn't used in any other trip
                var bus = await busRepo.GetByIdAsync(busId);
                if (bus != null)
                {
                    bus.RouteId = null;
                    busRepo.Update(bus);
                }
            }

            await _unitOfWork.Complete();
            return true;
        }
    }
}