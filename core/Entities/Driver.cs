using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Entities
{
	public class Driver : BaseEntity
	{
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string LicenseNumber { get; set; }
		public Buses Bus { get; set; }
	}
}
