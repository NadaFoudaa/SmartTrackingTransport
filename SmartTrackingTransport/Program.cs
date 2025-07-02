using API.SignalR;
using Core.DbContexts;
using Core.Helper;
using Infrastructure.Interfaces;
using Infrastructure.Repos;
using Infrastucture.DbContexts;
using Infrastucture.Entities;
using Infrastucture.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Services.Services.BusService;
using Services.Services.BusTripService;
using Services.Services.Busv2Service;
using Services.Services.DriverService;
using Services.Services.IEmailService;
using Services.Services.LostItemsService;
using Services.Services.StopsService;
using Services.Services.TokenService;
using Services.Services.TrackingService;
using Services.Services.TripService;
using Services.Services.Tripv2Service;
using Services.Services.UserService;
using SmartTrackingTransport.Extensions;
using SmartTrackingTransport.Mappings;
//using Services.HostedServices;


namespace SmartTrackingTransport
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            // Add services to the container.
            builder.Services.AddControllers();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
            });

            builder.Services.AddSignalR();
           
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddDbContext<TransportContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbConnection"));
            });
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddOpenApi();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSwaggerDocumentation();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IBusv2Repository, Busv2Repository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWorkv2, UnitOfWorkv2>();
            builder.Services.AddScoped<IBusv2Service, Busv2Service>();
            builder.Services.AddScoped<ITripv2Service, Tripv2Service>();
            builder.Services.AddScoped<IStopsService, StopsService>();
            builder.Services.AddScoped<ITrackingService, TrackingService>();
            builder.Services.AddScoped<IBusTripService, BusTripService>();
            builder.Services.AddScoped<ILostItemsService, LostItemService>();
            builder.Services.AddScoped<IDriverService, DriverService>();
            builder.Services.AddIdentityService(builder.Configuration);
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                await AppIdentityDbContextSeed.SeedRolesAsync(roleManager);
                await AppIdentityDbContextSeed.SeedDefaultAdminUserAsync(userManager);
            }

            app.MapHub<TrackingHub>("/trackingHub");

            app.MapDefaultEndpoints();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
