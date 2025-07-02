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

    public class TrackingDataConfiguration : IEntityTypeConfiguration<TrackingData>
    {
        public void Configure(EntityTypeBuilder<TrackingData> builder)
        {
            builder.Property(e => e.Latitude).HasPrecision(18, 7);
            builder.Property(e => e.Longitude).HasPrecision(18, 7);
        }
    }
}
