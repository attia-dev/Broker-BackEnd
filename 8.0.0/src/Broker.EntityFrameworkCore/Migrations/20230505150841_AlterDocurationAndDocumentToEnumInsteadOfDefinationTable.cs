using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class AlterDocurationAndDocumentToEnumInsteadOfDefinationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Definitions_DecorationId",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Definitions_DocumentId",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_DecorationId",
                table: "Advertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_DocumentId",
                table: "Advertisements");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "Advertisements",
                newName: "Document");

            migrationBuilder.RenameColumn(
                name: "DecorationId",
                table: "Advertisements",
                newName: "Decoration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Document",
                table: "Advertisements",
                newName: "DocumentId");

            migrationBuilder.RenameColumn(
                name: "Decoration",
                table: "Advertisements",
                newName: "DecorationId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_DecorationId",
                table: "Advertisements",
                column: "DecorationId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_DocumentId",
                table: "Advertisements",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Definitions_DecorationId",
                table: "Advertisements",
                column: "DecorationId",
                principalTable: "Definitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Definitions_DocumentId",
                table: "Advertisements",
                column: "DocumentId",
                principalTable: "Definitions",
                principalColumn: "Id");
        }
    }
}
