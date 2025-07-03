using Services.Services.SeatService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastucture.DbContexts;
using Infrastucture.Entities;
using Microsoft.EntityFrameworkCore;



namespace Services.Services.SeatService
{
    public class SeatService : ISeatService
    {
        private readonly TransportContext _context;

        public SeatService(TransportContext context)
        {
            _context = context;
        }

        public async Task<TripSeatLayoutDto> GetSeatLayoutAsync(int tripId, int userId)
        {
            var trip = await _context.Trip
                .Include(t => t.BusTrips).ThenInclude(bt => bt.Bus)
                .Include(t => t.Seats)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null) return null;

            var bus = trip.BusTrips.FirstOrDefault()?.Bus;

            return new TripSeatLayoutDto
            {
                TripId = trip.Id,
                StartTime = trip.StartTime,
                Bus = bus != null ? new BusInfoDto
                {
                    Model = bus.Model,
                    LicensePlate = bus.LicensePlate,
                    Capacity = bus.Capacity
                } : null,
                Seats = trip.Seats.Select(s => new SeatDto
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    IsReserved = s.IsReserved,
                    IsMine = s.UserId == userId
                }).ToList()
            };
        }

        public async Task<string> ReserveSeatAsync(ReserveSeatDto dto, int userId)
        {
            var seat = await _context.Seats
                .FirstOrDefaultAsync(s => s.TripId == dto.TripId && s.SeatNumber == dto.SeatNumber);

            if (seat == null)
                return "Seat not found.";

            if (seat.IsReserved)
                return "Seat already reserved.";

            var alreadyBooked = await _context.Seats
                .AnyAsync(s => s.TripId == dto.TripId && s.UserId == userId);

            if (alreadyBooked)
                return "You already reserved a seat in this trip.";

            seat.UserId = userId;
            seat.IsReserved = true;

            await _context.SaveChangesAsync();
            return "Seat reserved successfully.";
        }
    }
}
