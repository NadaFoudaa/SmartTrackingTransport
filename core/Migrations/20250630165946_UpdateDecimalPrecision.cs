using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "TrackingData",
                type: "decimal(18,7)",
                precision: 18,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "TrackingData",
                type: "decimal(18,7)",
                precision: 18,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Stops",
                type: "decimal(18,7)",
                precision: 18,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Stops",
                type: "decimal(18,7)",
                precision: 18,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentLongitude",
                table: "Bus",
                type: "decimal(18,7)",
                precision: 18,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentLatitude",
                table: "Bus",
                type: "decimal(18,7)",
                precision: 18,
                scale: 7,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "TrackingData",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,7)",
                oldPrecision: 18,
                oldScale: 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "TrackingData",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,7)",
                oldPrecision: 18,
                oldScale: 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Stops",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,7)",
                oldPrecision: 18,
                oldScale: 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Stops",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,7)",
                oldPrecision: 18,
                oldScale: 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentLongitude",
                table: "Bus",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,7)",
                oldPrecision: 18,
                oldScale: 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentLatitude",
                table: "Bus",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,7)",
                oldPrecision: 18,
                oldScale: 7);
        }
    }
}
