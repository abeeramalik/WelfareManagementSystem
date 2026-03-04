using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    /// <inheritdoc />
    public partial class MakeTransactionFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShelterDescription",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ItemDescription",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FoodDescription",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ClothesType",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ClothesDescription",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "WelfareFunds",
                keyColumn: "FundId",
                keyValue: 1,
                columns: new[] { "LastMonthlyReset", "LastUpdated" },
                values: new object[] { new DateTime(2025, 12, 2, 5, 38, 16, 420, DateTimeKind.Local).AddTicks(5729), new DateTime(2025, 12, 2, 5, 38, 16, 420, DateTimeKind.Local).AddTicks(5711) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShelterDescription",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemDescription",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FoodDescription",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClothesType",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClothesDescription",
                table: "DonorToWelfareTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "WelfareFunds",
                keyColumn: "FundId",
                keyValue: 1,
                columns: new[] { "LastMonthlyReset", "LastUpdated" },
                values: new object[] { new DateTime(2025, 12, 2, 3, 9, 0, 593, DateTimeKind.Local).AddTicks(5669), new DateTime(2025, 12, 2, 3, 9, 0, 593, DateTimeKind.Local).AddTicks(5641) });
        }
    }
}
