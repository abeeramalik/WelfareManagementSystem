using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatethewelfareToNGOReqeusts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WelfareToReceiverTransactions_AdminLogins_ApprovedByAdminId",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WelfareToReceiverTransactions_ReceiverRequests_RequestId",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "NgoResponse",
                table: "WelfareToNGORequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FoodUnit",
                table: "WelfareToNGORequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WelfareBalanceAfter",
                table: "DonorToWelfareTransactions",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.UpdateData(
                table: "WelfareFunds",
                keyColumn: "FundId",
                keyValue: 1,
                columns: new[] { "LastMonthlyReset", "LastUpdated" },
                values: new object[] { new DateTime(2025, 12, 6, 12, 33, 7, 480, DateTimeKind.Local).AddTicks(6668), new DateTime(2025, 12, 6, 12, 33, 7, 480, DateTimeKind.Local).AddTicks(6641) });

            migrationBuilder.AddForeignKey(
                name: "FK_WelfareToReceiverTransactions_AdminLogins_ApprovedByAdminId",
                table: "WelfareToReceiverTransactions",
                column: "ApprovedByAdminId",
                principalTable: "AdminLogins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WelfareToReceiverTransactions_ReceiverRequests_RequestId",
                table: "WelfareToReceiverTransactions",
                column: "RequestId",
                principalTable: "ReceiverRequests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WelfareToReceiverTransactions_AdminLogins_ApprovedByAdminId",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_WelfareToReceiverTransactions_ReceiverRequests_RequestId",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "NgoResponse",
                table: "WelfareToNGORequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FoodUnit",
                table: "WelfareToNGORequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "WelfareBalanceAfter",
                table: "DonorToWelfareTransactions",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "WelfareFunds",
                keyColumn: "FundId",
                keyValue: 1,
                columns: new[] { "LastMonthlyReset", "LastUpdated" },
                values: new object[] { new DateTime(2025, 12, 5, 3, 1, 10, 737, DateTimeKind.Local).AddTicks(7732), new DateTime(2025, 12, 5, 3, 1, 10, 737, DateTimeKind.Local).AddTicks(7710) });

            migrationBuilder.AddForeignKey(
                name: "FK_WelfareToReceiverTransactions_AdminLogins_ApprovedByAdminId",
                table: "WelfareToReceiverTransactions",
                column: "ApprovedByAdminId",
                principalTable: "AdminLogins",
                principalColumn: "AdminId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WelfareToReceiverTransactions_ReceiverRequests_RequestId",
                table: "WelfareToReceiverTransactions",
                column: "RequestId",
                principalTable: "ReceiverRequests",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
