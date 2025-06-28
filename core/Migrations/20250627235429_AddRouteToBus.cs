using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteToBus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bus_Routes_RouteId",
                table: "Bus");

            migrationBuilder.AddForeignKey(
                name: "FK_Bus_Routes_RouteId",
                table: "Bus",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bus_Routes_RouteId",
                table: "Bus");

            migrationBuilder.AddForeignKey(
                name: "FK_Bus_Routes_RouteId",
                table: "Bus",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id");
        }
    }
}