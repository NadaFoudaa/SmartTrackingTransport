using Services.Services.BusTripService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BusTripService
{
    public interface IBusTripService
    {
        Task<IEnumerable<BusTripDto>> GetAllAsync();
        Task<BusTripDto> GetByIdsAsync(int busId, int tripId);
        Task AddAsync(BusTripDto dto);
        Task DeleteAsync(int busId, int tripId);
        Task<bool> AssignBusToTripAsync(int tripId, int busId);
        Task<bool> UnassignBusFromTripAsync(int tripId, int busId);
    }
}
