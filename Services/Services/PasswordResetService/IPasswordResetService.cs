using System.Threading.Tasks;

namespace Services.Services.PasswordResetService
{
	public interface IPasswordResetService
	{
		Task<string> GenerateResetCodeAsync(string email);
		Task<bool> ValidateResetCodeAsync(string email, string code);
		Task<bool> MarkCodeAsUsedAsync(string email, string code);
		Task CleanupExpiredCodesAsync();
	}
}