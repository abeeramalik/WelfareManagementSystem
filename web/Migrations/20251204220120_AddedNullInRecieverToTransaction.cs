using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    /// <inheritdoc />
    public partial class AddedNullInRecieverToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LoanPurpose",
                table: "WelfareToReceiverTransactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "WelfareFunds",
                keyColumn: "FundId",
                keyValue: 1,
                columns: new[] { "LastMonthlyReset", "LastUpdated" },
                values: new object[] { new DateTime(2025, 12, 5, 3, 1, 10, 737, DateTimeKind.Local).AddTicks(7732), new DateTime(2025, 12, 5, 3, 1, 10, 737, DateTimeKind.Local).AddTicks(7710) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LoanPurpose",
                table: "WelfareToReceiverTransactions",
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
                values: new object[] { new DateTime(2025, 12, 4, 4, 39, 25, 376, DateTimeKind.Local).AddTicks(6083), new DateTime(2025, 12, 4, 4, 39, 25, 376, DateTimeKind.Local).AddTicks(5984) });
        }
    }
}
