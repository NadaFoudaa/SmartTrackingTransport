using Services.Services.LostItemsService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.LostItemsService
{
	public interface ILostItemsService
	{
		Task<IEnumerable<LostItemDto>> GetAllLostItemsAsync();
		Task<LostItemDto> GetLostItemByIdAsync(int id);
		Task<bool> AddLostItemAsync(ReportLostItemDto reportlostItemDto);
		Task<bool> DeleteLostItemAsync(int id);
	}
}
