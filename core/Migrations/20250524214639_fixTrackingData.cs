using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class fixTrackingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingData_Buses_BusId",
                table: "TrackingData");

            migrationBuilder.AddColumn<int>(
                name: "BusesId",
                table: "TrackingData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingData_BusesId",
                table: "TrackingData",
                column: "BusesId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingData_Bus_BusId",
                table: "TrackingData",
                column: "BusId",
                principalTable: "Bus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingData_Buses_BusesId",
                table: "TrackingData",
                column: "BusesId",
                principalTable: "Buses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingData_Bus_BusId",
                table: "TrackingData");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackingData_Buses_BusesId",
                table: "TrackingData");

            migrationBuilder.DropIndex(
                name: "IX_TrackingData_BusesId",
                table: "TrackingData");

            migrationBuilder.DropColumn(
                name: "BusesId",
                table: "TrackingData");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingData_Buses_BusId",
                table: "TrackingData",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
