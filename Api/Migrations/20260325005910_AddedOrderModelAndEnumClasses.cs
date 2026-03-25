using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderModelAndEnumClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery_type",
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
                    table.PrimaryKey("PK_delivery_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LotCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    WeekOfMonth = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "order_status",
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
                    table.PrimaryKey("PK_order_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "traceability_source_type",
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
                    table.PrimaryKey("PK_traceability_source_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lot_animal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LotId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnimalId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lot_animal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lot_animal_animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_lot_animal_lot_LotId",
                        column: x => x.LotId,
                        principalTable: "lot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    EstablishmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    QuickClientName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    OrderStatusId = table.Column<int>(type: "integer", nullable: false),
                    PrepDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveryDeadlineTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    DeliveryTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsUrgent = table.Column<bool>(type: "boolean", nullable: false),
                    OrderCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: true),
                    PaymentTypeId = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_delivery_type_DeliveryTypeId",
                        column: x => x.DeliveryTypeId,
                        principalTable: "delivery_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "establishment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_order_order_status_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "order_status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_payment_type_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "payment_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "route",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EstablishmentMenuItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestedQuantity = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    RequestedUnitId = table.Column<int>(type: "integer", nullable: false),
                    ApproxKgPerUnit = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    RequestNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PriceUnitId = table.Column<int>(type: "integer", nullable: false),
                    PreparedQuantity = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    PreparedUnitId = table.Column<int>(type: "integer", nullable: true),
                    PreparedWeightKg = table.Column<decimal>(type: "numeric(10,3)", nullable: true),
                    TraceabilitySourceTypeId = table.Column<int>(type: "integer", nullable: false),
                    AnimalId = table.Column<Guid>(type: "uuid", nullable: true),
                    LotId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderStatusId = table.Column<int>(type: "integer", nullable: false),
                    PreparedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PreparedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    PrepNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_item_animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "animal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_order_item_establishment_menu_item_EstablishmentMenuItemId",
                        column: x => x.EstablishmentMenuItemId,
                        principalTable: "establishment_menu_item",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_order_item_lot_LotId",
                        column: x => x.LotId,
                        principalTable: "lot",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_order_item_order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_item_order_status_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "order_status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_item_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_order_item_product_unit_PreparedUnitId",
                        column: x => x.PreparedUnitId,
                        principalTable: "product_unit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_order_item_product_unit_PriceUnitId",
                        column: x => x.PriceUnitId,
                        principalTable: "product_unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_item_product_unit_RequestedUnitId",
                        column: x => x.RequestedUnitId,
                        principalTable: "product_unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_item_traceability_source_type_TraceabilitySourceTypeId",
                        column: x => x.TraceabilitySourceTypeId,
                        principalTable: "traceability_source_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "delivery_type",
                columns: new[] { "Id", "Description", "IsActive", "Label", "Order" },
                values: new object[,]
                {
                    { 1, "Delivered to the client", true, "DELIVERY", 1 },
                    { 2, "Picked up by the client", true, "PICKUP", 2 }
                });

            migrationBuilder.InsertData(
                table: "order_status",
                columns: new[] { "Id", "Description", "IsActive", "Label", "Order" },
                values: new object[,]
                {
                    { 1, "Order created and awaiting preparation", true, "PENDING", 1 },
                    { 2, "Order is being prepared", true, "PREPARING", 2 },
                    { 3, "Order is ready for delivery or pickup", true, "READY", 3 },
                    { 4, "Order has been delivered", true, "DELIVERED", 4 }
                });

            migrationBuilder.InsertData(
                table: "traceability_source_type",
                columns: new[] { "Id", "Description", "IsActive", "Label", "Order" },
                values: new object[,]
                {
                    { 1, "No traceability source assigned", true, "NONE", 1 },
                    { 2, "Traceability comes from an animal", true, "ANIMAL", 2 },
                    { 3, "Traceability comes from a lot", true, "LOT", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_lot_animal_AnimalId",
                table: "lot_animal",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_lot_animal_LotId",
                table: "lot_animal",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_order_ClientId",
                table: "order",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_order_DeliveryTypeId",
                table: "order",
                column: "DeliveryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_order_EstablishmentId",
                table: "order",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_order_OrderStatusId",
                table: "order",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_order_PaymentTypeId",
                table: "order",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_order_RouteId",
                table: "order",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_AnimalId",
                table: "order_item",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_EstablishmentMenuItemId",
                table: "order_item",
                column: "EstablishmentMenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_LotId",
                table: "order_item",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_OrderId",
                table: "order_item",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_OrderStatusId",
                table: "order_item",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_PreparedUnitId",
                table: "order_item",
                column: "PreparedUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_PriceUnitId",
                table: "order_item",
                column: "PriceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_ProductId",
                table: "order_item",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_RequestedUnitId",
                table: "order_item",
                column: "RequestedUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_order_item_TraceabilitySourceTypeId",
                table: "order_item",
                column: "TraceabilitySourceTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lot_animal");

            migrationBuilder.DropTable(
                name: "order_item");

            migrationBuilder.DropTable(
                name: "lot");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "traceability_source_type");

            migrationBuilder.DropTable(
                name: "delivery_type");

            migrationBuilder.DropTable(
                name: "order_status");
        }
    }
}
