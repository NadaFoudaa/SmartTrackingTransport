using Infrastucture.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
	public class PasswordResetCode : BaseEntity
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Code { get; set; }

		[Required]
		public DateTime ExpiresAt { get; set; }

		public bool IsUsed { get; set; } = false;
	}
}