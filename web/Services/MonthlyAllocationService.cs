using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using web.Data;
using web.Models;
using Microsoft.EntityFrameworkCore; // Required for FindAsync

namespace web.Services
{
    // The Class name and Constructor name now match
    public class MonthlyAllocationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public MonthlyAllocationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // The service waits a few seconds before starting, just to ensure the app is fully ready
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Core work happens here
                await CheckAndAllocateMonthly();

                // Delay for 1 hour before checking again
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task CheckAndAllocateMonthly()
        {
            // Create a scope to get a DbContext instance (required for long-running services)
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WelfareDb>();

            // Find the main fund record (assuming ID 1 is the master record)
            // Use FirstOrDefaultAsync or FindAsync
            var fund = await context.WelfareFunds.FindAsync(1);
            if (fund == null) return; // Exit if the fund record doesn't exist

            // Check if it's a new month/year since the last reset
            if (DateTime.Now.Month != fund.LastMonthlyReset.Month ||
                DateTime.Now.Year != fund.LastMonthlyReset.Year)
            {
                // Log the state BEFORE allocation
                var allocationLog = new MonthlyAllocationLog
                {
                    AllocationDate = DateTime.Now,
                    BalanceBefore = fund.CurrentBalance,
                    FoodInventoryBefore = fund.FoodInventoryUnits,
                    MaleClothesInventoryBefore = fund.MaleClothesInventory,
                    FemaleClothesInventoryBefore = fund.FemaleClothesInventory,
                    KidsClothesInventoryBefore = fund.KidsClothesInventory,

                    // Log the amounts being allocated (from the fund settings)
                    MoneyAllocated = fund.MonthlyAllocation,
                    FoodAllocated = fund.MonthlyFoodAllocation,
                    FoodUnit = fund.FoodUnit,
                    MaleClothesAllocated = fund.MonthlyMaleClothesAllocation,
                    FemaleClothesAllocated = fund.MonthlyFemaleClothesAllocation,
                    KidsClothesAllocated = fund.MonthlyKidsClothesAllocation
                };

                //  Perform the ALLOCATION
                fund.CurrentBalance += fund.MonthlyAllocation;
                fund.FoodInventoryUnits += fund.MonthlyFoodAllocation;
                fund.MaleClothesInventory += fund.MonthlyMaleClothesAllocation;
                fund.FemaleClothesInventory += fund.MonthlyFemaleClothesAllocation;
                fund.KidsClothesInventory += fund.MonthlyKidsClothesAllocation;

                //  Update the reset marker and last updated time
                fund.LastMonthlyReset = DateTime.Now;
                fund.LastUpdated = DateTime.Now;

                // Finalize the Log and Save
                allocationLog.BalanceAfter = fund.CurrentBalance;
                allocationLog.FoodInventoryAfter = fund.FoodInventoryUnits;
                allocationLog.MaleClothesInventoryAfter = fund.MaleClothesInventory;
                allocationLog.FemaleClothesInventoryAfter = fund.FemaleClothesInventory;
                allocationLog.KidsClothesInventoryAfter = fund.KidsClothesInventory;
                allocationLog.Notes = "Automatic monthly allocation";

                context.MonthlyAllocationLogs.Add(allocationLog);
                await context.SaveChangesAsync();
            }
        }
    }
}