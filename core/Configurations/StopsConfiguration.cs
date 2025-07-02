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
    public class StopsConfiguration : IEntityTypeConfiguration<Stops>
    {
        public void Configure(EntityTypeBuilder<Stops> builder)
        {
            builder.Property(e => e.Latitude).HasPrecision(18, 7);
            builder.Property(e => e.Longitude).HasPrecision(18, 7);
        }
    }
}
