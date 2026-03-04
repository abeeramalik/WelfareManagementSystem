using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class NGOToWelfareTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey("NGOsLogin")]
        public int DonorNgoId { get; set; }
        public virtual NGOsLogin DonorNgo { get; set; }

        [Required]
        public string DonationType { get; set; } // Money, Clothes, Food, Shelter

        [Range(0, double.MaxValue)]
        public decimal MonetaryAmount { get; set; }

        public DateTime DonationDate { get; set; }

        // For Food Donations - ✅ ALL MADE NULLABLE
        public int? FoodQuantity { get; set; }
        public string? FoodUnit { get; set; }
        public string? FoodDescription { get; set; }

        // For Clothes Donations - ✅ ALL MADE NULLABLE
        public int? MaleClothesQuantity { get; set; }
        public int? FemaleClothesQuantity { get; set; }
        public int? KidsClothesQuantity { get; set; }
        public string? ClothesSize { get; set; }
        public string? ClothesType { get; set; }
        public string? ClothesDescription { get; set; }

        // For Shelter Donations - ✅ ALL MADE NULLABLE
        public int? ShelterBeds { get; set; }
        public string? ShelterDescription { get; set; }

        public string? ItemDescription { get; set; } // ✅ MADE NULLABLE

        // Balance after this transaction
        public decimal WelfareBalanceAfter { get; set; }
        public int? FoodInventoryAfter { get; set; }
        public int? MaleClothesInventoryAfter { get; set; }
        public int? FemaleClothesInventoryAfter { get; set; }
        public int? KidsClothesInventoryAfter { get; set; }
    }
}