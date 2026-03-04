using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class MonthlyAllocationLog
    {
        [Key]
        public int AllocationId { get; set; }

        public DateTime AllocationDate { get; set; }

        // Money allocation
        public decimal MoneyAllocated { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }

        // Food allocation
        public int FoodAllocated { get; set; }
        public int FoodInventoryBefore { get; set; }
        public int FoodInventoryAfter { get; set; }
        public string FoodUnit { get; set; }

        // Clothes allocation
        public int MaleClothesAllocated { get; set; }
        public int FemaleClothesAllocated { get; set; }
        public int KidsClothesAllocated { get; set; }

        public int MaleClothesInventoryBefore { get; set; }
        public int FemaleClothesInventoryBefore { get; set; }
        public int KidsClothesInventoryBefore { get; set; }

        public int MaleClothesInventoryAfter { get; set; }
        public int FemaleClothesInventoryAfter { get; set; }
        public int KidsClothesInventoryAfter { get; set; }

        public string Notes { get; set; }
    }
}