using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class makeAdvBuildingTypeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Definitions_PropertyId",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_PropertyId",
                table: "Advertisements");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "Advertisements",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Advertisements",
                newName: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_PropertyId",
                table: "Advertisements",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Definitions_PropertyId",
                table: "Advertisements",
                column: "PropertyId",
                principalTable: "Definitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
