using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class DonorToWelfareTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey(nameof(Donor))]
        public int DonorUserId { get; set; }
        public virtual UserLoginConfidentials Donor { get; set; }

        [Required]
        public string DonationType { get; set; } // Money, Clothes, Food, Shelter

        [Range(0, double.MaxValue)]
        public decimal MonetaryAmount { get; set; }

        public DateTime DonationDate { get; set; }

        // For Food Donations
        public int? FoodQuantity { get; set; }
        public string? FoodDescription { get; set; } // MADE NULLABLE

        // For Clothes Donations
        public int? MaleClothesQuantity { get; set; }
        public int? FemaleClothesQuantity { get; set; }
        public int? KidsClothesQuantity { get; set; }
        public string? ClothesType { get; set; } // MADE NULLABLE
        public string? ClothesDescription { get; set; } // MADE NULLABLE

        // For Shelter Donations
        public int? ShelterBeds { get; set; }
        public string? ShelterDescription { get; set; } // MADE NULLABLE

        public string? ItemDescription { get; set; } // MADE NULLABLE

        // Balance after this transaction
        public decimal WelfareBalanceAfter { get; set; }
        public int? FoodInventoryAfter { get; set; }
        public int? MaleClothesInventoryAfter { get; set; }
        public int? FemaleClothesInventoryAfter { get; set; }
        public int? KidsClothesInventoryAfter { get; set; }
    }
}