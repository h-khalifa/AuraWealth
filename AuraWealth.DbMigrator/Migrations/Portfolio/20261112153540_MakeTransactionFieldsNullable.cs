using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuraWealth.DbMigrator.Migrations.Portfolio
{
    /// <inheritdoc />
    public partial class MakeTransactionFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                schema: "portfolio",
                table: "TransactionRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                schema: "portfolio",
                table: "TransactionRecords",
                type: "numeric(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerUnit",
                schema: "portfolio",
                table: "TransactionRecords",
                type: "numeric(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Symbol",
                schema: "portfolio",
                table: "TransactionRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                schema: "portfolio",
                table: "TransactionRecords",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerUnit",
                schema: "portfolio",
                table: "TransactionRecords",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldNullable: true);
        }
    }
}
