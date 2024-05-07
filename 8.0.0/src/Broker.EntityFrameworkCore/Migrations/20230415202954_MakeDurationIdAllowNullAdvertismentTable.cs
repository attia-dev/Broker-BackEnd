using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class MakeDurationIdAllowNullAdvertismentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Durations_DurationId",
                table: "Advertisements");

            migrationBuilder.AlterColumn<long>(
                name: "DurationId",
                table: "Advertisements",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Durations_DurationId",
                table: "Advertisements",
                column: "DurationId",
                principalTable: "Durations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Durations_DurationId",
                table: "Advertisements");

            migrationBuilder.AlterColumn<long>(
                name: "DurationId",
                table: "Advertisements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Durations_DurationId",
                table: "Advertisements",
                column: "DurationId",
                principalTable: "Durations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
