using Infrastucture.DbContexts;
using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SmartTrackingTransport.Extensions
{
	public static class IdentityServiceExtension
	{
		public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration _config)
		{
			// Change from AddIdentityCore to AddIdentity to include all features including roles
			services.AddIdentity<AppUser, IdentityRole>(opt =>
			{
				// Password settings
				opt.Password.RequireDigit = true;
				opt.Password.RequireLowercase = true;
				opt.Password.RequireUppercase = true;
				opt.Password.RequireNonAlphanumeric = true;
				opt.Password.RequiredLength = 6;

				// User settings
				opt.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<AppIdentityDbContext>()
			.AddSignInManager<SignInManager<AppUser>>()
			.AddRoleManager<RoleManager<IdentityRole>>() // Add RoleManager
			.AddDefaultTokenProviders();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"])),
					ValidateIssuer = true,
					ValidIssuer = _config["Token:Issuer"],
					ValidateAudience = false
				};
			})
			.AddGoogle(googleOptions =>
			{
				googleOptions.ClientId = _config["Authentication:Google:ClientId"];
				googleOptions.ClientSecret = _config["Authentication:Google:ClientSecret"];
			});

			return services;
		}
	}
}