using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.UserService.Dto
{
	public class RegisterDto
	{
		[Required]
		public string DisplayName { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{6,}$",
		ErrorMessage = "Password must have at least 1 uppercase letter, 1 lowercase letter, 1 digit, 1 special character, and be at least 6 characters long.")]
		public string Password { get; set; }

	}
}
