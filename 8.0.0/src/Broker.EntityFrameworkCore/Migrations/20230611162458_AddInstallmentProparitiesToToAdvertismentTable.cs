using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class AddInstallmentProparitiesToToAdvertismentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DownPayment",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyInstallment",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NumOfYears",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "YearlyInstallment",
                table: "Advertisements",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownPayment",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "MonthlyInstallment",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "NumOfYears",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "YearlyInstallment",
                table: "Advertisements");
        }
    }
}
