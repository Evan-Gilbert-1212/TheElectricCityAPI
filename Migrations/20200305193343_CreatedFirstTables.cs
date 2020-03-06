using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TheElectricCityAPI.Migrations
{
    public partial class CreatedFirstTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SKU = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductType = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    ItemDescription = table.Column<string>(nullable: true),
                    NumberInStock = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    DateOrdered = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressCity = table.Column<string>(nullable: true),
                    AddressState = table.Column<string>(nullable: true),
                    AddressZip = table.Column<string>(nullable: true),
                    ManagerName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LocationItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryItemID = table.Column<int>(nullable: false),
                    NumberInStock = table.Column<int>(nullable: false),
                    LocationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LocationItems_InventoryItems_InventoryItemID",
                        column: x => x.InventoryItemID,
                        principalTable: "InventoryItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationItems_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationItems_InventoryItemID",
                table: "LocationItems",
                column: "InventoryItemID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationItems_LocationID",
                table: "LocationItems",
                column: "LocationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationItems");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
