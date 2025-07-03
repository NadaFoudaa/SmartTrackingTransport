using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
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
            var seatRepo = _unitOfWork.Repository<Seat>();

            var trip = await tripRepo.GetByIdAsync(tripId);
            var bus = await busRepo.GetByIdAsync(busId);

            if (trip == null || bus == null || trip.IsDeleted) return false;

            var alreadyAssigned = (await busTripRepo.FindAllAsync(bt => bt.BusId == busId && bt.TripsId == tripId)).Any();
            if (alreadyAssigned) return false;

            // Assign bus
            busTripRepo.Add(new BusTrip
            {
                BusId = busId,
                TripsId = tripId
            });

            // Update bus route
            bus.RouteId = trip.RouteId;
            busRepo.Update(bus);

            // Generate seats only if not already generated
            var existingSeats = await seatRepo.FindAllAsync(s => s.TripId == tripId);
            if (!existingSeats.Any())
            {
                var seats = new List<Seat>();
                for (int i = 1; i <= bus.Capacity; i++)
                {
                    seats.Add(new Seat
                    {
                        TripId = tripId,
                        SeatNumber = i.ToString("D2"),
                        IsReserved = false
                    });
                }

                await seatRepo.AddRangeAsync(seats);
            }

            await _unitOfWork.Complete();
            return true;
        }
        public async Task<bool> UnassignBusFromTripAsync(int tripId, int busId)
        {
            var busTripRepo = _unitOfWork.Repository<BusTrip>();
            var busRepo = _unitOfWork.Repository<Bus>();
            var seatRepo = _unitOfWork.Repository<Seat>();

            // 1. Find the mapping
            var mapping = (await busTripRepo.FindAllAsync(bt => bt.TripsId == tripId && bt.BusId == busId)).FirstOrDefault();
            if (mapping == null) return false;

            // 2. Remove the mapping
            busTripRepo.Delete(mapping);

            // 3. Clear RouteId if not used elsewhere
            var stillAssigned = (await busTripRepo.FindAllAsync(bt => bt.BusId == busId && bt.TripsId != tripId)).Any();
            if (!stillAssigned)
            {
                var bus = await busRepo.GetByIdAsync(busId);
                if (bus != null)
                {
                    bus.RouteId = null;
                    busRepo.Update(bus);
                }
            }

            // 4. Delete seats for this trip
            var seats = await seatRepo.FindAllAsync(s => s.TripId == tripId);
            if (seats.Any())
            {
                foreach (var seat in seats)
                {
                    seatRepo.Delete(seat);
                }
            }

            await _unitOfWork.Complete();
            return true;
        }
    }
}