using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.LostItemsService.DTO
{
	public class LostItemDto
	{
		public int Id { get; set; }
		public string BusNumber { get; set; }
		public string Description { get; set; }
		public string ContactName { get; set; }
		public string ContactPhone { get; set; }
		public string PhotoUrl { get; set; }
		public DateTime ReportedAt { get; set; }
	}
}
