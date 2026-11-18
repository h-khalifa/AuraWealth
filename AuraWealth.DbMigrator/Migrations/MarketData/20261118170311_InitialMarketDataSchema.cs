using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuraWealth.DbMigrator.Migrations.MarketData
{
    /// <inheritdoc />
    public partial class InitialMarketDataSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "market_data");

            migrationBuilder.CreateTable(
                name: "PriceTicks",
                schema: "market_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Symbol = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    ObservedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTicks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradableAssets",
                schema: "market_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Symbol = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AssetClass = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Quote_Price = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    Quote_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    Quote_ObservedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradableAssets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceTicks_InstrumentId",
                schema: "market_data",
                table: "PriceTicks",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTicks_Symbol_ObservedAtUtc",
                schema: "market_data",
                table: "PriceTicks",
                columns: new[] { "Symbol", "ObservedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_TradableAssets_Symbol",
                schema: "market_data",
                table: "TradableAssets",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceTicks",
                schema: "market_data");

            migrationBuilder.DropTable(
                name: "TradableAssets",
                schema: "market_data");
        }
    }
}
