using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedProductModelAndEnumClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_category",
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
                    table.PrimaryKey("PK_product_category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProductCategoryId = table.Column<int>(type: "integer", nullable: false),
                    DefaultUnitId = table.Column<int>(type: "integer", nullable: false),
                    DefaultVatRate = table.Column<decimal>(type: "numeric", nullable: true),
                    DefaultSellPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    InternalCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_product_category_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "product_category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_product_unit_DefaultUnitId",
                        column: x => x.DefaultUnitId,
                        principalTable: "product_unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "product_category",
                columns: new[] { "Id", "Description", "IsActive", "Label", "Order" },
                values: new object[,]
                {
                    { 1, "Produtos da categoria bovino", true, "Bovino", 1 },
                    { 2, "Produtos transformados", true, "Transformados", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_DefaultUnitId",
                table: "product",
                column: "DefaultUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_product_ProductCategoryId",
                table: "product",
                column: "ProductCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "product_category");
        }
    }
}
