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
	public class StopsAndRouteConfigurations : IEntityTypeConfiguration<RouteStop>
	{
		public void Configure(EntityTypeBuilder<RouteStop> builder)
		{
			builder.HasKey(rs => new { rs.RouteId, rs.StopId });

			// Define Relationships
			builder.HasOne(rs => rs.Route)
				   .WithMany(r => r.RouteStops)
				   .HasForeignKey(rs => rs.RouteId)
				   .OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(rs => rs.Stop)
				   .WithMany(s => s.RouteStops)
				   .HasForeignKey(rs => rs.StopId)
				   .OnDelete(DeleteBehavior.Cascade);

			// Order column (sequence of stops)
			builder.Property(rs => rs.Order)
				   .IsRequired();
		}
	}
}
