using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services.PasswordResetService
{
	public class PasswordResetService : IPasswordResetService
	{
		private readonly IUnitOfWorkv2 _unitOfWork;
		private readonly IGenericRepository<PasswordResetCode> _resetCodeRepository;
		private readonly Random _random;

		public PasswordResetService(IUnitOfWorkv2 unitOfWork, IGenericRepository<PasswordResetCode> resetCodeRepository)
		{
			_unitOfWork = unitOfWork;
			_resetCodeRepository = resetCodeRepository;
			_random = new Random();
		}

		public async Task<string> GenerateResetCodeAsync(string email)
		{
			// Clean up any existing expired codes for this email
			await CleanupExpiredCodesForEmailAsync(email);

			// Generate a 6-digit random code
			string code = _random.Next(100000, 999999).ToString();

			// Set expiration to 15 minutes from now
			var expiresAt = DateTime.UtcNow.AddMinutes(15);

			// Create new reset code
			var resetCode = new PasswordResetCode
			{
				Email = email,
				Code = code,
				ExpiresAt = expiresAt,
				IsUsed = false
			};

			await _resetCodeRepository.Add(resetCode);
			await _unitOfWork.Complete();

			return code;
		}

		public async Task<bool> ValidateResetCodeAsync(string email, string code)
		{
			var resetCode = await _resetCodeRepository.GetAllAsync();
			var validCode = resetCode
				.Where(rc => rc.Email == email &&
						   rc.Code == code &&
						   !rc.IsUsed &&
						   rc.ExpiresAt > DateTime.UtcNow)
				.FirstOrDefault();

			return validCode != null;
		}

		public async Task<bool> MarkCodeAsUsedAsync(string email, string code)
		{
			var resetCode = await _resetCodeRepository.GetAllAsync();
			var codeToMark =  resetCode
				.Where(rc => rc.Email == email && rc.Code == code)
				.FirstOrDefault();

			if (codeToMark == null)
				return false;

			codeToMark.IsUsed = true;
			await _unitOfWork.Complete();
			return true;
		}

		public async Task CleanupExpiredCodesAsync()
		{
			var expiredCodes = await _resetCodeRepository.GetAllAsync();
			var codesToDelete = expiredCodes
				.Where(rc => rc.ExpiresAt < DateTime.UtcNow || rc.IsUsed)
				.ToList();

			foreach (var code in codesToDelete)
			{
				 _resetCodeRepository.Delete(code);
			}

			await _unitOfWork.Complete();
		}

		private async Task CleanupExpiredCodesForEmailAsync(string email)
		{
			var existingCodes = await _resetCodeRepository.GetAllAsync();
			var codesToDelete = existingCodes
				.Where(rc => rc.Email == email)
				.ToList();

			foreach (var code in codesToDelete)
			{
				 _resetCodeRepository.Delete(code);
			}

			await _unitOfWork.Complete();
		}
	}
}