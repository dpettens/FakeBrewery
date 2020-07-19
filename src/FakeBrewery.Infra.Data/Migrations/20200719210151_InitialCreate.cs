using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeBrewery.Infra.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Breweries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breweries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Beers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Strength = table.Column<double>(nullable: false),
                    PriceWithoutVat = table.Column<double>(nullable: false),
                    BreweryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beers_Breweries_BreweryId",
                        column: x => x.BreweryId,
                        principalTable: "Breweries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Breweries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("cd876cae-ff5b-429d-970b-11af42900f1b"), "Abbaye de Leffe" },
                    { new Guid("91bff65f-96f2-4bd1-8b2e-eeaef2b46555"), "Abbaye d'Orval" },
                    { new Guid("a5a1d759-7471-431e-92c0-0f40c35bc855"), "Abbaye de Westmalle" },
                    { new Guid("d661055d-5c38-4201-b937-79b1b5d77f8f"), "Brasserie Bosteels" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Beers_BreweryId",
                table: "Beers",
                column: "BreweryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Beers");

            migrationBuilder.DropTable(
                name: "Breweries");
        }
    }
}
