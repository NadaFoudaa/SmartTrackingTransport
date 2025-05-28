using Infrastucture.DbContexts;
using Infrastucture.IdentityEntities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace SmartTrackingTransport.Helper
{
	public class ApplySeeding
	{
		public static async Task ApplySeedingAsync(WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var loggerFactory = services.GetRequiredService<ILoggerFactory>();
				var userManger = services.GetRequiredService<UserManager<AppUser>>();
				try
				{
					var identityContext = services.GetRequiredService<AppIdentityDbContext>();
					await AppIdentityContextSeed.SeedUserAsync(userManger);
				}
				catch (Exception ex) {
					var logger = loggerFactory.CreateLogger<AppIdentityContextSeed>();
					logger.LogError(ex.Message);
				}
			}
		}
	}
}
