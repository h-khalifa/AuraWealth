using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuraWealth.DbMigrator.Migrations.Portfolio
{
    /// <inheritdoc />
    public partial class InitialPortfolioSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "portfolio");

            migrationBuilder.CreateTable(
                name: "TraderProfiles",
                schema: "portfolio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CashBalance = table.Column<decimal>(type: "numeric(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraderProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionRecords",
                schema: "portfolio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TraderId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetHoldings",
                schema: "portfolio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TraderProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Symbol = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetHoldings", x => new { x.TraderProfileId, x.Id });
                    table.ForeignKey(
                        name: "FK_AssetHoldings_TraderProfiles_TraderProfileId",
                        column: x => x.TraderProfileId,
                        principalSchema: "portfolio",
                        principalTable: "TraderProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRecords_OccurredOn",
                schema: "portfolio",
                table: "TransactionRecords",
                column: "OccurredOn");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRecords_TraderId",
                schema: "portfolio",
                table: "TransactionRecords",
                column: "TraderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetHoldings",
                schema: "portfolio");

            migrationBuilder.DropTable(
                name: "TransactionRecords",
                schema: "portfolio");

            migrationBuilder.DropTable(
                name: "TraderProfiles",
                schema: "portfolio");
        }
    }
}
