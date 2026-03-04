using Microsoft.EntityFrameworkCore;
using web.Models;

namespace web.Data
{
    public class WelfareDb : DbContext
    {
        public WelfareDb(DbContextOptions<WelfareDb> options) : base(options)
        {
        }

        // Login tables
        public DbSet<UserLoginConfidentials> UserLoginConfidentials { get; set; }
        public DbSet<NGOsLogin> NGOsLogins { get; set; }
        public DbSet<AdminLogin> AdminLogins { get; set; }

        // Request tables
        public DbSet<ReceiverRequest> ReceiverRequests { get; set; }
        public DbSet<WelfareToNGORequest> WelfareToNGORequests { get; set; }

        // Transaction tables (separate flows)
        public DbSet<DonorToWelfareTransaction> DonorToWelfareTransactions { get; set; }
        public DbSet<NGOToWelfareTransaction> NGOToWelfareTransactions { get; set; }
        public DbSet<WelfareToReceiverTransaction> WelfareToReceiverTransactions { get; set; }

        // Fund management
        public DbSet<WelfareFund> WelfareFunds { get; set; }
        public DbSet<MonthlyAllocationLog> MonthlyAllocationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserLoginConfidentials unique constraints
            modelBuilder.Entity<UserLoginConfidentials>()
                .HasIndex(u => u.CNIC)
                .IsUnique();

            modelBuilder.Entity<UserLoginConfidentials>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // NGOsLogin unique constraints
            modelBuilder.Entity<NGOsLogin>()
                .HasIndex(n => n.Email)
                .IsUnique();

            // ReceiverRequest relationships
            modelBuilder.Entity<ReceiverRequest>()
                .HasOne(r => r.Receiver)
                .WithMany(u => u.ReceiverRequests)
                .HasForeignKey(r => r.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // DonorToWelfareTransaction relationships
            modelBuilder.Entity<DonorToWelfareTransaction>()
                .HasOne(d => d.Donor)
                .WithMany(u => u.DonorTransactions)
                .HasForeignKey(d => d.DonorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // NGOToWelfareTransaction relationships
            modelBuilder.Entity<NGOToWelfareTransaction>()
                .HasOne(n => n.DonorNgo)
                .WithMany(ngo => ngo.NGOTransactions)
                .HasForeignKey(n => n.DonorNgoId)
                .OnDelete(DeleteBehavior.Restrict);

            // WelfareToReceiverTransaction relationships
            modelBuilder.Entity<WelfareToReceiverTransaction>()
                .HasOne(w => w.Receiver)
                .WithMany()
                .HasForeignKey(w => w.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WelfareToReceiverTransaction>()
                .HasOne(w => w.ApprovedByAdmin)
                .WithMany(a => a.ReceiverApprovals)
                .HasForeignKey(w => w.ApprovedByAdminId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<WelfareToReceiverTransaction>()
                .HasOne(w => w.Request)
                .WithMany()
                .HasForeignKey(w => w.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // WelfareToNGORequest relationships
            modelBuilder.Entity<WelfareToNGORequest>()
                .HasOne(w => w.Admin)
                .WithMany(a => a.NGORequests)
                .HasForeignKey(w => w.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WelfareToNGORequest>()
                .HasOne(w => w.Ngo)
                .WithMany(n => n.WelfareRequests)
                .HasForeignKey(w => w.NgoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Precision for decimal properties
            modelBuilder.Entity<DonorToWelfareTransaction>()
                .Property(d => d.MonetaryAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DonorToWelfareTransaction>()
                .Property(d => d.WelfareBalanceAfter)
                .HasPrecision(18, 2);

            modelBuilder.Entity<NGOToWelfareTransaction>()
                .Property(n => n.MonetaryAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<NGOToWelfareTransaction>()
                .Property(n => n.WelfareBalanceAfter)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WelfareToReceiverTransaction>()
                .Property(w => w.MonetaryAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WelfareToReceiverTransaction>()
                .Property(w => w.WelfareBalanceAfter)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WelfareToReceiverTransaction>()
                .Property(w => w.MonthlyRepaymentAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ReceiverRequest>()
                .Property(r => r.RequestedAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WelfareToNGORequest>()
                .Property(r => r.RequestedAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WelfareToNGORequest>()
                .Property(r => r.FulfilledAmount)
                .HasPrecision(18, 2);

            // WelfareFund decimal precision
            modelBuilder.Entity<WelfareFund>()
                .Property(w => w.CurrentBalance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<WelfareFund>()
                .Property(w => w.MonthlyAllocation)
                .HasPrecision(18, 2);

            // MonthlyAllocationLog decimal precision
            modelBuilder.Entity<MonthlyAllocationLog>()
                .Property(m => m.MoneyAllocated)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyAllocationLog>()
                .Property(m => m.BalanceBefore)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyAllocationLog>()
                .Property(m => m.BalanceAfter)
                .HasPrecision(18, 2);

           

            modelBuilder.Entity<WelfareFund>().HasData(
                new WelfareFund
                {
                    FundId = 1,
                    CurrentBalance = 1000000,
                    MonthlyAllocation = 1000000,
                    LastUpdated = DateTime.Now,
                    LastMonthlyReset = DateTime.Now,
                    FoodInventoryUnits = 100000,
                    MonthlyFoodAllocation = 50000,
                    FoodUnit = "rations",
                    MaleClothesInventory = 1000,
                    FemaleClothesInventory = 1000,
                    KidsClothesInventory = 500,
                    MonthlyMaleClothesAllocation = 500,
                    MonthlyFemaleClothesAllocation = 500,
                    MonthlyKidsClothesAllocation = 300,
                    ShelterCapacity = 50,
                    ShelterOccupied = 0,
                    ShelterAvailable = 50
                }
            );
        }
    }
}