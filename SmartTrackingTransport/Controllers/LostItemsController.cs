using Microsoft.AspNetCore.Mvc;
using Services.Services.LostItemsService;
using Services.Services.LostItemsService.DTO;

namespace SmartTrackingTransport.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LostItemsController : ControllerBase
	{
		private readonly ILostItemsService _lostItemService;

		public LostItemsController(ILostItemsService lostItemService)
		{
			_lostItemService = lostItemService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<LostItemDto>>> GetAllLostItems()
		{
			var items = await _lostItemService.GetAllLostItemsAsync();
			return Ok(items);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<LostItemDto>> GetLostItem(int id)
		{
			var item = await _lostItemService.GetLostItemByIdAsync(id);
			if (item == null) return NotFound();
			return Ok(item);
		}

		[HttpPost]
		public async Task<ActionResult> AddLostItem([FromBody] ReportLostItemDto reportlostItemDto)
		{
			var success = await _lostItemService.AddLostItemAsync(reportlostItemDto);
			if (!success) return BadRequest("Failed to add lost item.");
			return Ok("Lost item added successfully.");
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteLostItem(int id)
		{
			var success = await _lostItemService.DeleteLostItemAsync(id);
			if (!success) return NotFound();
			return Ok("Lost item deleted successfully.");
		}
	}
}

