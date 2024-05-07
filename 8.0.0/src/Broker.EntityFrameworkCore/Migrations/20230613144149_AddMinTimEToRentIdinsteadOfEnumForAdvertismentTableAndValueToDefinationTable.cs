using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class AddMinTimEToRentIdinsteadOfEnumForAdvertismentTableAndValueToDefinationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MinTimeToBook",
                table: "Advertisements",
                newName: "MinTimeToBookForChaletId");

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "Definitions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_MinTimeToBookForChaletId",
                table: "Advertisements",
                column: "MinTimeToBookForChaletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Definitions_MinTimeToBookForChaletId",
                table: "Advertisements",
                column: "MinTimeToBookForChaletId",
                principalTable: "Definitions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Definitions_MinTimeToBookForChaletId",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_MinTimeToBookForChaletId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Definitions");

            migrationBuilder.RenameColumn(
                name: "MinTimeToBookForChaletId",
                table: "Advertisements",
                newName: "MinTimeToBook");
        }
    }
}
