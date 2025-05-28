using AutoMapper;
using Infrastructure.Interfaces;
using Infrastucture.Entities;
using Services.Services.LostItemsService.DTO;

namespace Services.Services.LostItemsService
{
	public class LostItemService : ILostItemsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public LostItemService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<LostItemDto>> GetAllLostItemsAsync()
		{
			var items = await _unitOfWork.Repository<LostItem>().GetAllAsync();
			return _mapper.Map<IEnumerable<LostItemDto>>(items);
		}

		public async Task<LostItemDto> GetLostItemByIdAsync(int id)
		{
			var item = await _unitOfWork.Repository<LostItem>().GetByIdAsync(id);
			return item == null ? null : _mapper.Map<LostItemDto>(item);
		}

		public async Task<bool> AddLostItemAsync(ReportLostItemDto reportlostItemDto)
		{
			var item = _mapper.Map<LostItem>(reportlostItemDto);
			await _unitOfWork.Repository<LostItem>().Add(item);
			return await _unitOfWork.Complete() > 0;
		}

		public async Task<bool> DeleteLostItemAsync(int id)
		{
			var item = await _unitOfWork.Repository<LostItem>().GetByIdAsync(id);
			if (item == null) return false;

			_unitOfWork.Repository<LostItem>().Delete(item);
			return await _unitOfWork.Complete() > 0;
		}
	}
}