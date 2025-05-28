using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configurations
{
    public class BusTripConfiguration : IEntityTypeConfiguration<BusTrip>
    {
        public void Configure(EntityTypeBuilder<BusTrip> builder)
        {
            builder.HasKey(bt => new { bt.BusId, bt.TripsId });

            builder.HasOne(bt => bt.Bus)
                .WithMany(b => b.BusTrips)
                .HasForeignKey(bt => bt.BusId);

            builder.HasOne(bt => bt.Trip)
                .WithMany(t => t.BusTrips)
                .HasForeignKey(bt => bt.TripsId);
        }
    }
}
