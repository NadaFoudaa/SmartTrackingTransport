using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Entities
{
	public class LostItem : BaseEntity
	{
		public string ImagePath { get; set; } // Path to uploaded image
		public string BusNumber { get; set; }
		public string Description { get; set; }
		public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
		public string Name { get; set; } // Person reporting
		public string Phone { get; set; }
	}
}
