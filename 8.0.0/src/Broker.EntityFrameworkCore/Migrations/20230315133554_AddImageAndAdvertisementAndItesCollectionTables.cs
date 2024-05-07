using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Broker.Migrations
{
    /// <inheritdoc />
    public partial class AddImageAndAdvertisementAndItesCollectionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    Compound = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    FloorsNumber = table.Column<int>(type: "int", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingArea = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChaletType = table.Column<int>(type: "int", nullable: true),
                    AgreementStatus = table.Column<int>(type: "int", nullable: true),
                    BuildingStatus = table.Column<int>(type: "int", nullable: true),
                    LandingStatus = table.Column<int>(type: "int", nullable: true),
                    UsingFor = table.Column<int>(type: "int", nullable: true),
                    StreetWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BuildingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Rooms = table.Column<int>(type: "int", nullable: true),
                    Reception = table.Column<int>(type: "int", nullable: true),
                    Balcony = table.Column<int>(type: "int", nullable: true),
                    Kitchen = table.Column<int>(type: "int", nullable: true),
                    Toilet = table.Column<int>(type: "int", nullable: true),
                    NumUnits = table.Column<int>(type: "int", nullable: true),
                    NumPartitions = table.Column<int>(type: "int", nullable: true),
                    OfficesNum = table.Column<int>(type: "int", nullable: true),
                    OfficesFloors = table.Column<int>(type: "int", nullable: true),
                    DurationId = table.Column<long>(type: "bigint", nullable: false),
                    SeekerId = table.Column<long>(type: "bigint", nullable: true),
                    OwnerId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    BrokerPersonId = table.Column<long>(type: "bigint", nullable: true),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    IsApprove = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeaturedAd = table.Column<bool>(type: "bit", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentFacility = table.Column<int>(type: "int", nullable: true),
                    MrMrs = table.Column<int>(type: "int", nullable: true),
                    AdvertiseMakerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdvertiseMaker = table.Column<int>(type: "int", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWhatsApped = table.Column<bool>(type: "bit", nullable: false),
                    SecondMobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactRegisterInTheAccount = table.Column<bool>(type: "bit", nullable: false),
                    Furnished = table.Column<bool>(type: "bit", nullable: true),
                    Elevator = table.Column<bool>(type: "bit", nullable: true),
                    Parking = table.Column<bool>(type: "bit", nullable: true),
                    ParkingSpace = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Garden = table.Column<bool>(type: "bit", nullable: true),
                    GardenArea = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Pool = table.Column<bool>(type: "bit", nullable: true),
                    Shop = table.Column<bool>(type: "bit", nullable: true),
                    ShopsNumber = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Advertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advertisements_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Advertisements_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Advertisements_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Advertisements_BrokerPersons_BrokerPersonId",
                        column: x => x.BrokerPersonId,
                        principalTable: "BrokerPersons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Advertisements_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisements_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Advertisements_Durations_DurationId",
                        column: x => x.DurationId,
                        principalTable: "Durations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisements_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Advertisements_Seekers_SeekerId",
                        column: x => x.SeekerId,
                        principalTable: "Seekers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Images_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Images_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementDecorations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DecorationId = table.Column<int>(type: "int", nullable: false),
                    AdvertisementId = table.Column<long>(type: "bigint", nullable: false),
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
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    AdvertisementId = table.Column<long>(type: "bigint", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "AdvertisementFacilities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    AdvertisementId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_AdvertisementFacilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementFacilities_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementFacilities_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementFacilities_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementFacilities_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementFacilities_Definitions_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Definitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageId = table.Column<long>(type: "bigint", nullable: false),
                    AdvertisementId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_AdvertisementImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementImages_AbpUsers_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementImages_AbpUsers_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementImages_AbpUsers_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementImages_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
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

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementFacilities_AdvertisementId",
                table: "AdvertisementFacilities",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementFacilities_CreatorUserId",
                table: "AdvertisementFacilities",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementFacilities_DeleterUserId",
                table: "AdvertisementFacilities",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementFacilities_FacilityId",
                table: "AdvertisementFacilities",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementFacilities_LastModifierUserId",
                table: "AdvertisementFacilities",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementImages_AdvertisementId",
                table: "AdvertisementImages",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementImages_CreatorUserId",
                table: "AdvertisementImages",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementImages_DeleterUserId",
                table: "AdvertisementImages",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementImages_ImageId",
                table: "AdvertisementImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementImages_LastModifierUserId",
                table: "AdvertisementImages",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_BrokerPersonId",
                table: "Advertisements",
                column: "BrokerPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CityId",
                table: "Advertisements",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CompanyId",
                table: "Advertisements",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CreatorUserId",
                table: "Advertisements",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_DeleterUserId",
                table: "Advertisements",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_DurationId",
                table: "Advertisements",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_LastModifierUserId",
                table: "Advertisements",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_OwnerId",
                table: "Advertisements",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_SeekerId",
                table: "Advertisements",
                column: "SeekerId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CreatorUserId",
                table: "Images",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_DeleterUserId",
                table: "Images",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_LastModifierUserId",
                table: "Images",
                column: "LastModifierUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementDecorations");

            migrationBuilder.DropTable(
                name: "AdvertisementDocuments");

            migrationBuilder.DropTable(
                name: "AdvertisementFacilities");

            migrationBuilder.DropTable(
                name: "AdvertisementImages");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
