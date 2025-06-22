using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.IdentityEntities
{
	public class Address
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[Required]
		public string AppUserId { get; set; }
		public AppUser AppUser { get; set; }
	}
}
