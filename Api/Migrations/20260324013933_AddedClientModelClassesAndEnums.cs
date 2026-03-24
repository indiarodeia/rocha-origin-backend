using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedClientModelClassesAndEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payment_type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product_unit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "route",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    VatNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    BillingAddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    PaymentTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_address_BillingAddressId",
                        column: x => x.BillingAddressId,
                        principalTable: "address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_client_payment_type_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "payment_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "establishment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DeliveryAddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: true),
                    LocalContactPhone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_establishment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_establishment_address_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "address",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_establishment_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_establishment_route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "route",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "establishment_menu_item",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EstablishmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_establishment_menu_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_establishment_menu_item_establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "establishment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "payment_type",
                columns: new[] { "Id", "Description", "IsActive", "Label", "Order" },
                values: new object[,]
                {
                    { 1, "Immediate payment", true, "Immediate", 1 },
                    { 2, "Credit payment", true, "Credit", 2 },
                    { 3, "Customer-defined payment", true, "Customer", 3 }
                });

            migrationBuilder.InsertData(
                table: "product_unit",
                columns: new[] { "Id", "Description", "IsActive", "Label", "Order" },
                values: new object[,]
                {
                    { 1, "Kilogram", true, "KG", 1 },
                    { 2, "Unit", true, "UN", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_client_BillingAddressId",
                table: "client",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_client_PaymentTypeId",
                table: "client",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_establishment_ClientId",
                table: "establishment",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_establishment_DeliveryAddressId",
                table: "establishment",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_establishment_RouteId",
                table: "establishment",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_establishment_menu_item_EstablishmentId",
                table: "establishment_menu_item",
                column: "EstablishmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "establishment_menu_item");

            migrationBuilder.DropTable(
                name: "product_unit");

            migrationBuilder.DropTable(
                name: "establishment");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "route");

            migrationBuilder.DropTable(
                name: "payment_type");
        }
    }
}
