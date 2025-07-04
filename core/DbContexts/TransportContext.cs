﻿using Core.Entities;
using Infrastucture.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DbContexts
{
	public class TransportContext : DbContext
	{
		public TransportContext(DbContextOptions<TransportContext> options) : base(options)
		{
		}
        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Stops> Stops { get; set; }

        public DbSet<Trips> Trip { get; set; }
        public DbSet<Bus> Bus { get; set; }
        public DbSet<Seat> Seats { get; set; }

        public DbSet<TrackingData> TrackingData { get; set; }
        public DbSet<LostItem> LostItems { get; set; }

		public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TrackingData>(entity =>
            {
                entity.Property(e => e.Latitude).HasPrecision(18, 7);
                entity.Property(e => e.Longitude).HasPrecision(18, 7);
            });



            modelBuilder.Entity<Stops>(entity =>
            {
                entity.Property(e => e.Latitude).HasPrecision(18, 7);
                entity.Property(e => e.Longitude).HasPrecision(18, 7);
            });
        }
	}
}
