using Microsoft.OpenApi.Models;

namespace SmartTrackingTransport.Extensions
{
	public static class SwaggerServiceExtension
	{
		public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api" , Version =  "v1" });
				var securityScheme = new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name="Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "bearer",
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "bearer"
					}
				};
				c.AddSecurityDefinition("bearer", securityScheme);
				var securityRequirment = new OpenApiSecurityRequirement
				{
					{securityScheme , new[] { "bearer" } }
				};
				c.AddSecurityRequirement(securityRequirment);

			});
			return services;
		}
	}
}
