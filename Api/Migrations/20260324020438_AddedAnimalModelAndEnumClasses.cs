using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedAnimalModelAndEnumClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "animal_species",
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
                    table.PrimaryKey("PK_animal_species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    VatNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VatRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    ExplorationId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProfilePicture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CoverPicture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Certifications = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_supplier_address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "address",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "animal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AnimalSpeciesId = table.Column<int>(type: "integer", nullable: false),
                    AnimalIdentification = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SlaughterDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DispatchDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BirthPlace = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RearingPlace = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Breed = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    ColdWeightKg = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    AgeMonths = table.Column<int>(type: "integer", nullable: true),
                    Ph = table.Column<decimal>(type: "numeric(4,2)", nullable: true),
                    EuropConformation = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    EuropFatClass = table.Column<int>(type: "integer", nullable: true),
                    EuropCategory = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    EuropRaw = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CarcassPhotoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SlaughterhouseRef = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_animal_animal_species_AnimalSpeciesId",
                        column: x => x.AnimalSpeciesId,
                        principalTable: "animal_species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_animal_supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "animal_species",
                columns: new[] { "Id", "Description", "IsActive", "Label", "Order" },
                values: new object[,]
                {
                    { 1, "Espécie bovina", true, "BOVINO", 1 },
                    { 2, "Espécie ovina", true, "OVINO", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_animal_AnimalSpeciesId",
                table: "animal",
                column: "AnimalSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_animal_SupplierId",
                table: "animal",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_AddressId",
                table: "supplier",
                column: "AddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "animal");

            migrationBuilder.DropTable(
                name: "animal_species");

            migrationBuilder.DropTable(
                name: "supplier");
        }
    }
}
