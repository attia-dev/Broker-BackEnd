using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentIdAndDecorationIdAndDinningToAdvertismentTableAndRemoveTablesAdVertismentDecorationAdVertismentDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementDecorations");

            migrationBuilder.DropTable(
                name: "AdvertisementDocuments");

            migrationBuilder.AddColumn<int>(
                name: "DecorationId",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dinning",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "Advertisements",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "DecorationId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "Dinning",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Advertisements");

            migrationBuilder.CreateTable(
                name: "AdvertisementDecorations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementId = table.Column<long>(type: "bigint", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DecorationId = table.Column<int>(type: "int", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementDecorations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementDecorations_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementDecorations_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementDecorations_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementDecorations_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementDecorations_Definitions_DecorationId",
                        column: x => x.DecorationId,
                        principalTable: "Definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementId = table.Column<long>(type: "bigint", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementDocuments_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementDocuments_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementDocuments_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementDocuments_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementDocuments_Definitions_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDecorations_AdvertisementId",
                table: "AdvertisementDecorations",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDecorations_CreatorUserId",
                table: "AdvertisementDecorations",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDecorations_DecorationId",
                table: "AdvertisementDecorations",
                column: "DecorationId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDecorations_DeleterUserId",
                table: "AdvertisementDecorations",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDecorations_LastModifierUserId",
                table: "AdvertisementDecorations",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDocuments_AdvertisementId",
                table: "AdvertisementDocuments",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDocuments_CreatorUserId",
                table: "AdvertisementDocuments",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDocuments_DeleterUserId",
                table: "AdvertisementDocuments",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDocuments_DocumentId",
                table: "AdvertisementDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementDocuments_LastModifierUserId",
                table: "AdvertisementDocuments",
                column: "LastModifierUserId");
        }
    }
}
