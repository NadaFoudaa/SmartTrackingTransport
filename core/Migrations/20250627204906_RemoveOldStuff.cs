using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldStuff : Migration
    {
        /// <inheritdoc />
            protected override void Up(MigrationBuilder migrationBuilder)
            {

                // Drop Buses table
                migrationBuilder.DropTable(name: "Buses");

                // Drop Trips table
                migrationBuilder.DropTable(name: "Trips");
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                // Recreate Buses table (if needed on rollback)
                migrationBuilder.CreateTable(
                    name: "Buses",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        DriverId = table.Column<int>(type: "int", nullable: true),
                        RouteId = table.Column<int>(type: "int", nullable: true),
                        Capacity = table.Column<int>(type: "int", nullable: false),
                        CurrentLatitude = table.Column<decimal>(type: "decimal(10,6)", nullable: false),
                        CurrentLongitude = table.Column<decimal>(type: "decimal(10,6)", nullable: false),
                        LicensePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                        Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                        Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Buses", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Buses_Drivers_DriverId",
                            column: x => x.DriverId,
                            principalTable: "Drivers",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Restrict);
                        table.ForeignKey(
                            name: "FK_Buses_Routes_RouteId",
                            column: x => x.RouteId,
                            principalTable: "Routes",
                            principalColumn: "Id");
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_Buses_DriverId",
                    table: "Buses",
                    column: "DriverId",
                    unique: true,
                    filter: "[DriverId] IS NOT NULL");

                migrationBuilder.CreateIndex(
                    name: "IX_Buses_RouteId",
                    table: "Buses",
                    column: "RouteId");

                // Recreate Trips table (if needed on rollback)
                migrationBuilder.CreateTable(
                    name: "Trips",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        BusId = table.Column<int>(type: "int", nullable: false),
                        RouteId = table.Column<int>(type: "int", nullable: false),
                        StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                        EndTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_Trips", x => x.Id);
                        table.ForeignKey(
                            name: "FK_Trips_Buses_BusId",
                            column: x => x.BusId,
                            principalTable: "Buses",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                        table.ForeignKey(
                            name: "FK_Trips_Routes_RouteId",
                            column: x => x.RouteId,
                            principalTable: "Routes",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_Trips_BusId",
                    table: "Trips",
                    column: "BusId");

                migrationBuilder.CreateIndex(
                    name: "IX_Trips_RouteId",
                    table: "Trips",
                    column: "RouteId");

                // Re-add FK from TrackingData to Buses
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

