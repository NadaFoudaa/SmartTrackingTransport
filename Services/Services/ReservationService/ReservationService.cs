using AutoMapper;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
using Services.Services.ReservationService.DTO;
using Services.Services.ReservationService.Infrastructure.Interfaces;

namespace Infrastructure.Services
{
	/*
	public class ReservationService : IReservationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ReservationService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<SeatReservationDto> AddReservationAsync(BookSeatDto bookSeatDto)
		{
			// 1. Check if Trip Exists
			var trip = await _unitOfWork.Repository<Trip>().GetByIdAsync(bookSeatDto.TripId);
			if (trip == null)
				throw new InvalidOperationException("Trip not found.");

			// 2. Check if Trip is still active (optional: if you have a Status field for Trip)
			// if (trip.Status == "Cancelled" || trip.Status == "Completed")
			//    throw new InvalidOperationException("Cannot reserve on this trip.");


			// 4. Check Seat Availability
			var existingSeatReservation = await _unitOfWork.Repository<Reservation>()
				.FindAsync(r => r.TripId == bookSeatDto.TripId && r.SeatNumber == bookSeatDto.SeatNumber);

			if (existingSeatReservation != null)
				throw new InvalidOperationException("Seat already booked.");

			// 5. Check if Trip Capacity is reached
			var totalReservations = await _unitOfWork.Repository<Reservation>()
				.CountAsync(r => r.TripId == bookSeatDto.TripId);

			if (totalReservations >= trip.Bus.Capacity)
				throw new InvalidOperationException("No available seats on this trip.");

			// 6. Prevent User from Reserving Same Trip Twice
			var userReservation = await _unitOfWork.Repository<Reservation>()
				.FindAsync(r => r.TripId == bookSeatDto.TripId && r.UserId == bookSeatDto.UserId);

			if (userReservation != null)
				throw new InvalidOperationException("You have already reserved a seat for this trip.");

			// 7. Map and Save
			var reservation = _mapper.Map<Reservation>(bookSeatDto);
			await _unitOfWork.Repository<Reservation>().Add(reservation);

			// Return the SeatReservationDto after the reservation is added
			var seatReservationDto = _mapper.Map<SeatReservationDto>(reservation);
			return seatReservationDto;
		}

		public async Task<IEnumerable<SeatReservationDto>> GetAllReservationsAsync()
		{
			var reservations = await _unitOfWork.Repository<Reservation>()
				.FindAllAsync();

			return _mapper.Map<IEnumerable<SeatReservationDto>>(reservations);
		}

		public async Task<SeatReservationDto> GetReservationByIdAsync(int id)
		{
			var reservation = await _unitOfWork.Repository<Reservation>().GetByIdAsync(id);
			if (reservation == null)
				throw new InvalidOperationException("Reservation not found.");

			return _mapper.Map<SeatReservationDto>(reservation);
		}

		public async Task<bool> UpdateReservationAsync(int id, SeatReservationDto seatReservationDto)
		{
			var reservation = await _unitOfWork.Repository<Reservation>().GetByIdAsync(id);
			if (reservation == null)
				return false;

			// Update reservation details here as needed
			reservation.SeatNumber = seatReservationDto.SeatNumber;
			reservation.PassengerName = seatReservationDto.PassengerName;

			// More properties can be updated depending on your logic.

			_unitOfWork.Repository<Reservation>().Update(reservation);
			return await _unitOfWork.Complete() > 0;
		}

		public async Task<bool> CancelReservationAsync(int reservationId)
		{
			var reservation = await _unitOfWork.Repository<Reservation>().GetByIdAsync(reservationId);
			if (reservation == null)
				return false;

			_unitOfWork.Repository<Reservation>().Delete(reservation);
			return await _unitOfWork.Complete() > 0;
		}
	}
	*/
}
