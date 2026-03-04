using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    /// <inheritdoc />
    public partial class FixReceiverModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClothesSize",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropColumn(
                name: "FamilyMembers",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropColumn(
                name: "FemaleClothesGiven",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropColumn(
                name: "FemaleClothesInventoryAfter",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropColumn(
                name: "FoodInventoryAfter",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropColumn(
                name: "FoodQuantityGiven",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropColumn(
                name: "ClothesSize",
                table: "ReceiverRequests");

            migrationBuilder.DropColumn(
                name: "FoodUnit",
                table: "ReceiverRequests");

            migrationBuilder.RenameColumn(
                name: "ShelterDays",
                table: "WelfareToReceiverTransactions",
                newName: "RepaymentMonths");

            migrationBuilder.RenameColumn(
                name: "MaleClothesInventoryAfter",
                table: "WelfareToReceiverTransactions",
                newName: "MaleClothesProvided");

            migrationBuilder.RenameColumn(
                name: "MaleClothesGiven",
                table: "WelfareToReceiverTransactions",
                newName: "KidsClothesProvided");

            migrationBuilder.RenameColumn(
                name: "KidsClothesInventoryAfter",
                table: "WelfareToReceiverTransactions",
                newName: "FoodUnitsProvided");

            migrationBuilder.RenameColumn(
                name: "KidsClothesGiven",
                table: "WelfareToReceiverTransactions",
                newName: "FemaleClothesProvided");

            migrationBuilder.RenameColumn(
                name: "FoodUnit",
                table: "WelfareToReceiverTransactions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ClothesType",
                table: "WelfareToReceiverTransactions",
                newName: "LoanPurpose");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedByAdminId",
                table: "WelfareToReceiverTransactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyRepaymentAmount",
                table: "WelfareToReceiverTransactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShelterEndDate",
                table: "WelfareToReceiverTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShelterStartDate",
                table: "WelfareToReceiverTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoanPurpose",
                table: "ReceiverRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ClothesType",
                table: "ReceiverRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "WelfareFunds",
                keyColumn: "FundId",
                keyValue: 1,
                columns: new[] { "LastMonthlyReset", "LastUpdated" },
                values: new object[] { new DateTime(2025, 12, 4, 4, 39, 25, 376, DateTimeKind.Local).AddTicks(6083), new DateTime(2025, 12, 4, 4, 39, 25, 376, DateTimeKind.Local).AddTicks(5984) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyRepaymentAmount",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropColumn(
                name: "ShelterEndDate",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.DropColumn(
                name: "ShelterStartDate",
                table: "WelfareToReceiverTransactions");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "WelfareToReceiverTransactions",
                newName: "FoodUnit");

            migrationBuilder.RenameColumn(
                name: "RepaymentMonths",
                table: "WelfareToReceiverTransactions",
                newName: "ShelterDays");

            migrationBuilder.RenameColumn(
                name: "MaleClothesProvided",
                table: "WelfareToReceiverTransactions",
                newName: "MaleClothesInventoryAfter");

            migrationBuilder.RenameColumn(
                name: "LoanPurpose",
                table: "WelfareToReceiverTransactions",
                newName: "ClothesType");

            migrationBuilder.RenameColumn(
                name: "KidsClothesProvided",
                table: "WelfareToReceiverTransactions",
                newName: "MaleClothesGiven");

            migrationBuilder.RenameColumn(
                name: "FoodUnitsProvided",
                table: "WelfareToReceiverTransactions",
                newName: "KidsClothesInventoryAfter");

            migrationBuilder.RenameColumn(
                name: "FemaleClothesProvided",
                table: "WelfareToReceiverTransactions",
                newName: "KidsClothesGiven");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedByAdminId",
                table: "WelfareToReceiverTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClothesSize",
                table: "WelfareToReceiverTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FamilyMembers",
                table: "WelfareToReceiverTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FemaleClothesGiven",
                table: "WelfareToReceiverTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FemaleClothesInventoryAfter",
                table: "WelfareToReceiverTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoodInventoryAfter",
                table: "WelfareToReceiverTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoodQuantityGiven",
                table: "WelfareToReceiverTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoanPurpose",
                table: "ReceiverRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClothesType",
                table: "ReceiverRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClothesSize",
                table: "ReceiverRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FoodUnit",
                table: "ReceiverRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "WelfareFunds",
                keyColumn: "FundId",
                keyValue: 1,
                columns: new[] { "LastMonthlyReset", "LastUpdated" },
                values: new object[] { new DateTime(2025, 12, 2, 5, 38, 16, 420, DateTimeKind.Local).AddTicks(5729), new DateTime(2025, 12, 2, 5, 38, 16, 420, DateTimeKind.Local).AddTicks(5711) });
        }
    }
}
