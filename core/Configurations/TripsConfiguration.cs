using Core.Entities;
using Infrastucture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configurations
{
    public class TripsConfiguration : IEntityTypeConfiguration<Trips>
    {
        public void Configure(EntityTypeBuilder<Trips> builder)
        {
            builder.HasKey(t => t.Id);


           builder
    .HasOne(t => t.Route)
    .WithMany(r => r.Trip)
    .HasForeignKey(t => t.RouteId)
    .OnDelete(DeleteBehavior.Restrict);

            // Trips - Driver (optional)
            builder.HasOne(t => t.Driver)
                   .WithMany(d => d.Trip)
                   .HasForeignKey(t => t.DriverId)
                   .OnDelete(DeleteBehavior.SetNull); // or Restrict if you prefer

            builder.Property(t => t.Status)
                   .HasConversion<string>() // Store enums as string
                   .IsRequired();

            builder.Property(t => t.IsDeleted)
                   .HasDefaultValue(false);
        }
    }
}
