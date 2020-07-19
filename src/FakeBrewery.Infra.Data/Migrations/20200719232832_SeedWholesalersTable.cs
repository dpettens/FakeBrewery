using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeBrewery.Infra.Data.Migrations
{
    public partial class SeedWholesalersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Wholesalers",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("28cade6d-4d40-4fa2-96f5-e535a07aad7b"), "BeerLovers' Shop" });

            migrationBuilder.InsertData(
                table: "Wholesalers",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("9779f2fa-6f60-4fa9-9b18-28fb2505be6e"), "Beer Market" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Wholesalers",
                keyColumn: "Id",
                keyValue: new Guid("28cade6d-4d40-4fa2-96f5-e535a07aad7b"));

            migrationBuilder.DeleteData(
                table: "Wholesalers",
                keyColumn: "Id",
                keyValue: new Guid("9779f2fa-6f60-4fa9-9b18-28fb2505be6e"));
        }
    }
}
