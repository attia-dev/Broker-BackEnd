using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class packageAndBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Balance",
                table: "Seekers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PackageId",
                table: "Seekers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Balance",
                table: "Owners",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PackageId",
                table: "Owners",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Balance",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Balance",
                table: "BrokerPersons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PackageId",
                table: "BrokerPersons",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seekers_PackageId",
                table: "Seekers",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_PackageId",
                table: "Owners",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerPersons_PackageId",
                table: "BrokerPersons",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerPersons_Packages_PackageId",
                table: "BrokerPersons",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Packages_PackageId",
                table: "Owners",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seekers_Packages_PackageId",
                table: "Seekers",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerPersons_Packages_PackageId",
                table: "BrokerPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Packages_PackageId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Seekers_Packages_PackageId",
                table: "Seekers");

            migrationBuilder.DropIndex(
                name: "IX_Seekers_PackageId",
                table: "Seekers");

            migrationBuilder.DropIndex(
                name: "IX_Owners_PackageId",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_BrokerPersons_PackageId",
                table: "BrokerPersons");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Seekers");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Seekers");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "BrokerPersons");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "BrokerPersons");
        }
    }
}
