using Infrastucture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configurations
{
	public class BusesConfigurations : IEntityTypeConfiguration<Buses>
	{
		
		public void Configure(EntityTypeBuilder<Buses> builder)
		{
			builder.ToTable("Buses");

			builder.HasKey(b => b.Id);

			builder.Property(b => b.LicensePlate)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(b => b.Model)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(b => b.Capacity)
				.IsRequired();

			builder.Property(b => b.Status)
				.HasMaxLength(20)
				.HasDefaultValue("Active"); // Default status

			builder.Property(b => b.CurrentLatitude)
				.HasPrecision(10, 6);

			builder.Property(b => b.CurrentLongitude)
				.HasPrecision(10, 6);

			builder.HasOne(b => b.Driver)
				.WithOne(d => d.Bus)
				.HasForeignKey<Buses>(b => b.DriverId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(b => b.Trips)
				.WithOne(t => t.Bus)
				.HasForeignKey(t => t.BusId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
