using AutoMapper;
using Core.Entities;
using Infrastucture.Entities;
using Infrastucture.IdentityEntities;
using Services.Services.BusService.DTO;
using Services.Services.BusTripService.DTO;
using Services.Services.Busv2Service.DTO;
using Services.Services.DriverService.DTO;
using Services.Services.LostItemsService.DTO;
using Services.Services.RouteService.Dto;
using Services.Services.SeatService.DTO;
using Services.Services.StopsService.DTO;
using Services.Services.TrackingService.DTO;
using Services.Services.TripService.DTO;
using Services.Services.Tripv2Service.DTO;
using Services.Services.UserService.Dto;
using System.Linq;
using Route = Infrastucture.Entities.Route;

namespace SmartTrackingTransport.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<AppUser, UserDto>();

            CreateMap<RegisterDto, AppUser>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));

            // Trip mappings
            CreateMap<CRUDTripDto, Trips>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Route, o => o.Ignore())
                .ForMember(d => d.Driver, o => o.Ignore())
                .ForMember(d => d.BusTrips, o => o.Ignore());

            CreateMap<CRUDDriverDto, Driver>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Buses, o => o.Ignore())
                .ForMember(d => d.Trip, o => o.Ignore());

            CreateMap<Driver, DriverDto>();

            // Stop mappings
            CreateMap<Stops, StopsDto>();

            CreateMap<StopsDto, Stops>()
                .ForMember(d => d.RouteStops, o => o.Ignore());

            // Route mappings
            CreateMap<Route, RouteDto>()
                .ForMember(d => d.RouteId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Origin, o => o.MapFrom(s => s.Origin))
                .ForMember(d => d.Destination, o => o.MapFrom(s => s.Destination))
                .ForMember(d => d.Stops, o => o.MapFrom(s => s.RouteStops
                    .OrderBy(rs => rs.Order)
                    .Select(rs => rs.Stop.Name)
                    .ToList()));

            CreateMap<RouteDto, Route>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.RouteId))
                .ForMember(d => d.Origin, o => o.MapFrom(s => s.Origin))
                .ForMember(d => d.Destination, o => o.MapFrom(s => s.Destination))
                .ForMember(d => d.RouteStops, o => o.Ignore())
                .ForMember(d => d.Trip, o => o.Ignore())
                .ForMember(d => d.Buses, o => o.Ignore());
            CreateMap<RouteStop, StopsDto>()
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Stop.Name))
    .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Stop.Latitude))
    .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Stop.Longitude));

            // LostItem mappings
            CreateMap<LostItem, LostItemDto>()
                .ForMember(d => d.ContactName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ContactPhone, o => o.MapFrom(s => s.Phone))
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.ImagePath))
                .ForMember(d => d.BusNumber, o => o.MapFrom(s => s.BusNumber));

            CreateMap<LostItemDto, LostItem>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.ContactName))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.ContactPhone))
                .ForMember(d => d.ImagePath, o => o.MapFrom(s => s.PhotoUrl))
                .ForMember(d => d.BusNumber, o => o.MapFrom(s => s.BusNumber));

            CreateMap<LostItem, ReportLostItemDto>()
                .ForMember(d => d.ContactName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ContactPhone, o => o.MapFrom(s => s.Phone))
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.ImagePath))
                .ForMember(d => d.BusNumber, o => o.MapFrom(s => s.BusNumber));

            CreateMap<ReportLostItemDto, LostItem>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.ContactName))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.ContactPhone))
                .ForMember(d => d.ImagePath, o => o.MapFrom(s => s.PhotoUrl))
                .ForMember(d => d.BusNumber, o => o.MapFrom(s => s.BusNumber));

            // BusV2 mappings
            CreateMap<Bus, Busv2Dto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.LicensePlate, o => o.MapFrom(s => s.LicensePlate))
                .ForMember(d => d.Capacity, o => o.MapFrom(s => s.Capacity))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
                .ForMember(d => d.Model, o => o.MapFrom(s => s.Model))
                .ForMember(d => d.Origin, o => o.MapFrom(s => GetOriginName(s)))
                .ForMember(d => d.Destination, o => o.MapFrom(s => GetDestinationName(s)));

            CreateMap<Busv2Dto, Bus>()
                .ForMember(d => d.Driver, o => o.Ignore())
                .ForMember(d => d.Route, o => o.Ignore());

            CreateMap<Bus, Busv2AbstractDto>()
                .ForMember(d => d.BusNumber, o => o.MapFrom(s => s.LicensePlate))
                .ForMember(d => d.Origin, o => o.MapFrom(s => GetOriginName(s)))
                .ForMember(d => d.Destination, o => o.MapFrom(s => GetDestinationName(s)));
            // Trip
            CreateMap<CRUDTripDto, Trips>();
            CreateMap<Trips, TripOperationDto>();
            CreateMap<Trips, Tripv2Dto>();
            CreateMap<TripOperationDto, Tripv2Dto>();


            // Bus
            CreateMap<Bus, Busv2Dto>().ReverseMap();

            // Seat
            CreateMap<Seat, SeatDto>();
            CreateMap<SeatDto, Seat>();
            CreateMap<Bus, Busv2TripDetailsDto>()
                .ForMember(d => d.BusNumber, o => o.MapFrom(s => s.LicensePlate))
                .ForMember(d => d.Origin, o => o.MapFrom(s => GetOriginName(s)))
                .ForMember(d => d.Destination, o => o.MapFrom(s => GetDestinationName(s)))
                .ForMember(d => d.StartTime, o => o.MapFrom(s =>
                    s.BusTrips
                        .Select(bt => bt.Trip)
                        .OrderByDescending(t => t.StartTime)
                        .FirstOrDefault().StartTime))
                .ForMember(d => d.EndTime, o => o.MapFrom(s =>
                    s.BusTrips
                        .Select(bt => bt.Trip)
                        .OrderByDescending(t => t.StartTime)
                        .FirstOrDefault().EndTime))
                .ForMember(d => d.Stops, o => o.MapFrom(s =>
                    s.Route.RouteStops
                        .OrderBy(rs => rs.Order)
                        .Select(rs => new StopTimev2Dto
                        {
                            Stop = rs.Stop.Name,
                            Time = s.BusTrips
                                .Select(bt => bt.Trip)
                                .OrderByDescending(t => t.StartTime)
                                .FirstOrDefault().StartTime
                                .AddMinutes(rs.Order * 20)
                        })))
                .ForMember(d => d.LifeTrack, o => o.MapFrom(s =>
                    s.TrackingData
                        .OrderBy(td => td.Timestamp)
                        .Select(td => new LocationPointv2Dto
                        {
                            Latitude = td.Latitude,
                            Longitude = td.Longitude,
                            Timestamp = td.Timestamp
                        })));

            CreateMap<Bus, Busv2TripsDto>()
                .ForMember(d => d.BusNumber, o => o.MapFrom(s => s.LicensePlate))
                .ForMember(d => d.Origin, o => o.MapFrom(s => GetOriginName(s)))
                .ForMember(d => d.Destination, o => o.MapFrom(s => GetDestinationName(s)));

            // Tripv2
            CreateMap<Trips, Tripv2Dto>()
                .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BusIds, opt => opt.MapFrom(src => src.BusTrips.Select(bt => bt.BusId)));

            CreateMap<BusTrip, BusTripDto>().ReverseMap();

            CreateMap<Bus, LocationDto>()
    .ForMember(dest => dest.BusId, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.CurrentLatitude))
    .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.CurrentLongitude))
    .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.DriverId));
        }

        private string GetOriginName(Bus bus)
        {
            if (bus.Route?.RouteStops == null || !bus.Route.RouteStops.Any())
                return string.Empty;

            var firstStop = bus.Route.RouteStops.OrderBy(rs => rs.Order).FirstOrDefault();
            return firstStop?.Stop?.Name ?? string.Empty;
        }

        private string GetDestinationName(Bus bus)
        {
            if (bus.Route?.RouteStops == null || !bus.Route.RouteStops.Any())
                return string.Empty;

            var lastStop = bus.Route.RouteStops.OrderByDescending(rs => rs.Order).FirstOrDefault();
            return lastStop?.Stop?.Name ?? string.Empty;
        }
    }
}