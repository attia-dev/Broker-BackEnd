using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class AddDurationBuildingTablesAndRemovePropartyBuildingTablefromDurationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Durations");

            migrationBuilder.CreateTable(
                name: "DurationBuildingTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DurationId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DurationBuildingTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DurationBuildingTypes_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DurationBuildingTypes_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DurationBuildingTypes_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DurationBuildingTypes_Durations_DurationId",
                        column: x => x.DurationId,
                        principalTable: "Durations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DurationBuildingTypes_CreatorUserId",
                table: "DurationBuildingTypes",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DurationBuildingTypes_DeleterUserId",
                table: "DurationBuildingTypes",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DurationBuildingTypes_DurationId",
                table: "DurationBuildingTypes",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_DurationBuildingTypes_LastModifierUserId",
                table: "DurationBuildingTypes",
                column: "LastModifierUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DurationBuildingTypes");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Durations",
                type: "int",
                nullable: true);
        }
    }
}
