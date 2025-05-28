using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class secmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "TrackingData");

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Buses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Buses_RouteId",
                table: "Buses",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Routes_RouteId",
                table: "Buses",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Routes_RouteId",
                table: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_Buses_RouteId",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Buses");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "TrackingData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
