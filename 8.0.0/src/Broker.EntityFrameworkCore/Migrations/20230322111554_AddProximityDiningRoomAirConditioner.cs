using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class AddProximityDiningRoomAirConditioner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AirConditioner",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DiningRoom",
                table: "Advertisements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Officies",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProximityToTheSea",
                table: "Advertisements",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirConditioner",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "DiningRoom",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Officies",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ProximityToTheSea",
                table: "Advertisements");
        }
    }
}
