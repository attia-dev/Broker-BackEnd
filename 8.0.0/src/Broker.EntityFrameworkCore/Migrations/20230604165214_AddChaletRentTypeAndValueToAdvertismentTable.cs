using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class AddChaletRentTypeAndValueToAdvertismentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChaletRentType",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ChaletRentValue",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChaletRentType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "ChaletRentValue",
                table: "Advertisements");
        }
    }
}
