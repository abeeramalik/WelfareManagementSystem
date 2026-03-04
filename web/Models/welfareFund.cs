using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class WelfareFund
    {
        [Key]
        public int FundId { get; set; }

        [Required]
        public decimal CurrentBalance { get; set; }

        public decimal MonthlyAllocation { get; set; } // 1,000,000 PKR

        public DateTime LastUpdated { get; set; }

        public DateTime LastMonthlyReset { get; set; }

        // Food Inventory - stored as total ration units
        public int FoodInventoryUnits { get; set; } // Total food units (e.g., 100000 rations)
        public int MonthlyFoodAllocation { get; set; } // Monthly food units added (e.g., 50000 rations)
        public string FoodUnit { get; set; } // "rations", "kg", "packages" etc.

        // Clothes Inventory - categorized by type
        public int MaleClothesInventory { get; set; }
        public int FemaleClothesInventory { get; set; }
        public int KidsClothesInventory { get; set; }

        // Monthly clothes allocation
        public int MonthlyMaleClothesAllocation { get; set; }
        public int MonthlyFemaleClothesAllocation { get; set; }
        public int MonthlyKidsClothesAllocation { get; set; }

        // Shelter Capacity
        public int ShelterCapacity { get; set; }
        public int ShelterOccupied { get; set; }
        public int ShelterAvailable { get; set; }
    }
}