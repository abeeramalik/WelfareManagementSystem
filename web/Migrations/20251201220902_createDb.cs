using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    /// <inheritdoc />
    public partial class createDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "UserLoginConfidentials",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserLoginConfidentials",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserLoginConfidentials",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "UserLoginConfidentials",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "UserLoginConfidentials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserLoginConfidentials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "UserLoginConfidentials",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CNIC",
                table: "UserLoginConfidentials",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "UserLoginConfidentials",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserLoginConfidentials",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "UserLoginConfidentials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationName",
                table: "NGOsLogins",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "NGOsLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "NGOsLogins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "NGOsLogins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "NGOsLogins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "NGOsLogins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AdminLogins",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DonorToWelfareTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorUserId = table.Column<int>(type: "int", nullable: false),
                    DonationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonetaryAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DonationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FoodQuantity = table.Column<int>(type: "int", nullable: true),
                    FoodDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaleClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    FemaleClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    KidsClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    ClothesType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClothesDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShelterBeds = table.Column<int>(type: "int", nullable: true),
                    ShelterDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WelfareBalanceAfter = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FoodInventoryAfter = table.Column<int>(type: "int", nullable: true),
                    MaleClothesInventoryAfter = table.Column<int>(type: "int", nullable: true),
                    FemaleClothesInventoryAfter = table.Column<int>(type: "int", nullable: true),
                    KidsClothesInventoryAfter = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorToWelfareTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_DonorToWelfareTransactions_UserLoginConfidentials_DonorUserId",
                        column: x => x.DonorUserId,
                        principalTable: "UserLoginConfidentials",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyAllocationLogs",
                columns: table => new
                {
                    AllocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllocationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoneyAllocated = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceBefore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FoodAllocated = table.Column<int>(type: "int", nullable: false),
                    FoodInventoryBefore = table.Column<int>(type: "int", nullable: false),
                    FoodInventoryAfter = table.Column<int>(type: "int", nullable: false),
                    FoodUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaleClothesAllocated = table.Column<int>(type: "int", nullable: false),
                    FemaleClothesAllocated = table.Column<int>(type: "int", nullable: false),
                    KidsClothesAllocated = table.Column<int>(type: "int", nullable: false),
                    MaleClothesInventoryBefore = table.Column<int>(type: "int", nullable: false),
                    FemaleClothesInventoryBefore = table.Column<int>(type: "int", nullable: false),
                    KidsClothesInventoryBefore = table.Column<int>(type: "int", nullable: false),
                    MaleClothesInventoryAfter = table.Column<int>(type: "int", nullable: false),
                    FemaleClothesInventoryAfter = table.Column<int>(type: "int", nullable: false),
                    KidsClothesInventoryAfter = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyAllocationLogs", x => x.AllocationId);
                });

            migrationBuilder.CreateTable(
                name: "NGOToWelfareTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorNgoId = table.Column<int>(type: "int", nullable: false),
                    DonationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonetaryAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DonationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FoodQuantity = table.Column<int>(type: "int", nullable: true),
                    FoodUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaleClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    FemaleClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    KidsClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    ClothesSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClothesType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClothesDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShelterBeds = table.Column<int>(type: "int", nullable: true),
                    ShelterDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WelfareBalanceAfter = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FoodInventoryAfter = table.Column<int>(type: "int", nullable: true),
                    MaleClothesInventoryAfter = table.Column<int>(type: "int", nullable: true),
                    FemaleClothesInventoryAfter = table.Column<int>(type: "int", nullable: true),
                    KidsClothesInventoryAfter = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NGOToWelfareTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_NGOToWelfareTransactions_NGOsLogins_DonorNgoId",
                        column: x => x.DonorNgoId,
                        principalTable: "NGOsLogins",
                        principalColumn: "NgoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceiverRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FulfilledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FamilyMembers = table.Column<int>(type: "int", nullable: true),
                    FoodQuantity = table.Column<int>(type: "int", nullable: true),
                    FoodUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaleClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    FemaleClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    KidsClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    ClothesSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClothesType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShelterDurationDays = table.Column<int>(type: "int", nullable: true),
                    RequiredRooms = table.Column<int>(type: "int", nullable: true),
                    LoanPurpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepaymentMonths = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiverRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_ReceiverRequests_UserLoginConfidentials_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "UserLoginConfidentials",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WelfareFunds",
                columns: table => new
                {
                    FundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyAllocation = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastMonthlyReset = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FoodInventoryUnits = table.Column<int>(type: "int", nullable: false),
                    MonthlyFoodAllocation = table.Column<int>(type: "int", nullable: false),
                    FoodUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaleClothesInventory = table.Column<int>(type: "int", nullable: false),
                    FemaleClothesInventory = table.Column<int>(type: "int", nullable: false),
                    KidsClothesInventory = table.Column<int>(type: "int", nullable: false),
                    MonthlyMaleClothesAllocation = table.Column<int>(type: "int", nullable: false),
                    MonthlyFemaleClothesAllocation = table.Column<int>(type: "int", nullable: false),
                    MonthlyKidsClothesAllocation = table.Column<int>(type: "int", nullable: false),
                    ShelterCapacity = table.Column<int>(type: "int", nullable: false),
                    ShelterOccupied = table.Column<int>(type: "int", nullable: false),
                    ShelterAvailable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelfareFunds", x => x.FundId);
                });

            migrationBuilder.CreateTable(
                name: "WelfareToNGORequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    NgoId = table.Column<int>(type: "int", nullable: false),
                    RequestType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FoodQuantity = table.Column<int>(type: "int", nullable: true),
                    FoodUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaleClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    FemaleClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    KidsClothesQuantity = table.Column<int>(type: "int", nullable: true),
                    ShelterBeds = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgoResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FulfilledAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FulfilledFoodQuantity = table.Column<int>(type: "int", nullable: true),
                    FulfilledMaleClothes = table.Column<int>(type: "int", nullable: true),
                    FulfilledFemaleClothes = table.Column<int>(type: "int", nullable: true),
                    FulfilledKidsClothes = table.Column<int>(type: "int", nullable: true),
                    FulfilledShelterBeds = table.Column<int>(type: "int", nullable: true),
                    FulfilledDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelfareToNGORequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_WelfareToNGORequests_AdminLogins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AdminLogins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WelfareToNGORequests_NGOsLogins_NgoId",
                        column: x => x.NgoId,
                        principalTable: "NGOsLogins",
                        principalColumn: "NgoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WelfareToReceiverTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByAdminId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonetaryAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FamilyMembers = table.Column<int>(type: "int", nullable: true),
                    FoodQuantityGiven = table.Column<int>(type: "int", nullable: true),
                    FoodUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaleClothesGiven = table.Column<int>(type: "int", nullable: true),
                    FemaleClothesGiven = table.Column<int>(type: "int", nullable: true),
                    KidsClothesGiven = table.Column<int>(type: "int", nullable: true),
                    ClothesSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClothesType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShelterDays = table.Column<int>(type: "int", nullable: true),
                    RoomsAllocated = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WelfareBalanceAfter = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FoodInventoryAfter = table.Column<int>(type: "int", nullable: true),
                    MaleClothesInventoryAfter = table.Column<int>(type: "int", nullable: true),
                    FemaleClothesInventoryAfter = table.Column<int>(type: "int", nullable: true),
                    KidsClothesInventoryAfter = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelfareToReceiverTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_WelfareToReceiverTransactions_AdminLogins_ApprovedByAdminId",
                        column: x => x.ApprovedByAdminId,
                        principalTable: "AdminLogins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WelfareToReceiverTransactions_ReceiverRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "ReceiverRequests",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WelfareToReceiverTransactions_UserLoginConfidentials_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "UserLoginConfidentials",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "WelfareFunds",
                columns: new[] { "FundId", "CurrentBalance", "FemaleClothesInventory", "FoodInventoryUnits", "FoodUnit", "KidsClothesInventory", "LastMonthlyReset", "LastUpdated", "MaleClothesInventory", "MonthlyAllocation", "MonthlyFemaleClothesAllocation", "MonthlyFoodAllocation", "MonthlyKidsClothesAllocation", "MonthlyMaleClothesAllocation", "ShelterAvailable", "ShelterCapacity", "ShelterOccupied" },
                values: new object[] { 1, 1000000m, 1000, 100000, "rations", 500, new DateTime(2025, 12, 2, 3, 9, 0, 593, DateTimeKind.Local).AddTicks(5669), new DateTime(2025, 12, 2, 3, 9, 0, 593, DateTimeKind.Local).AddTicks(5641), 1000, 1000000m, 500, 50000, 300, 500, 50, 50, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginConfidentials_CNIC",
                table: "UserLoginConfidentials",
                column: "CNIC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginConfidentials_Email",
                table: "UserLoginConfidentials",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NGOsLogins_Email",
                table: "NGOsLogins",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonorToWelfareTransactions_DonorUserId",
                table: "DonorToWelfareTransactions",
                column: "DonorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NGOToWelfareTransactions_DonorNgoId",
                table: "NGOToWelfareTransactions",
                column: "DonorNgoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiverRequests_ReceiverId",
                table: "ReceiverRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_WelfareToNGORequests_AdminId",
                table: "WelfareToNGORequests",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_WelfareToNGORequests_NgoId",
                table: "WelfareToNGORequests",
                column: "NgoId");

            migrationBuilder.CreateIndex(
                name: "IX_WelfareToReceiverTransactions_ApprovedByAdminId",
                table: "WelfareToReceiverTransactions",
                column: "ApprovedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_WelfareToReceiverTransactions_ReceiverId",
                table: "WelfareToReceiverTransactions",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_WelfareToReceiverTransactions_RequestId",
                table: "WelfareToReceiverTransactions",
                column: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonorToWelfareTransactions");

            migrationBuilder.DropTable(
                name: "MonthlyAllocationLogs");

            migrationBuilder.DropTable(
                name: "NGOToWelfareTransactions");

            migrationBuilder.DropTable(
                name: "WelfareFunds");

            migrationBuilder.DropTable(
                name: "WelfareToNGORequests");

            migrationBuilder.DropTable(
                name: "WelfareToReceiverTransactions");

            migrationBuilder.DropTable(
                name: "ReceiverRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginConfidentials_CNIC",
                table: "UserLoginConfidentials");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginConfidentials_Email",
                table: "UserLoginConfidentials");

            migrationBuilder.DropIndex(
                name: "IX_NGOsLogins_Email",
                table: "NGOsLogins");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserLoginConfidentials");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "UserLoginConfidentials");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "NGOsLogins");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "NGOsLogins");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "NGOsLogins");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "NGOsLogins");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AdminLogins");

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "UserLoginConfidentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "UserLoginConfidentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserLoginConfidentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "UserLoginConfidentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "UserLoginConfidentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserLoginConfidentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "UserLoginConfidentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CNIC",
                table: "UserLoginConfidentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "UserLoginConfidentials",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationName",
                table: "NGOsLogins",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "NGOsLogins",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
