using Core.Entities;
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
    public class BusConfigurations : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            builder.HasOne(b => b.Route)
                   .WithMany(r => r.Buses)
                   .HasForeignKey(b => b.RouteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
