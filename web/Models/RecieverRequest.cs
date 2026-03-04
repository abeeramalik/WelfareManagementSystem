using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class ReceiverRequest
    {
        [Key]
        public int RequestId { get; set; }

        [ForeignKey("UserLoginConfidentials")]
        public int ReceiverId { get; set; }
        public virtual UserLoginConfidentials Receiver { get; set; }

        [Required]
        public string RequestType { get; set; } // Shelter, Clothes, Loan, Food

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0")]
        public decimal RequestedAmount { get; set; }

        [Required]
        public string Status { get; set; } // Pending, Approved, Rejected, Fulfilled

        public DateTime RequestDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? FulfilledDate { get; set; }

        // For Food Requests
        public int? FamilyMembers { get; set; }
        public int? FoodQuantity { get; set; }

        // For Clothes Requests
        public int? MaleClothesQuantity { get; set; }
        public int? FemaleClothesQuantity { get; set; }
        public int? KidsClothesQuantity { get; set; }
        public string? ClothesType { get; set; } // Winter, Summer, Formal, Casual

        // For Shelter Requests
        public int? ShelterDurationDays { get; set; }
        public int? RequiredRooms { get; set; }

        // For Loan Requests
        public string? LoanPurpose { get; set; }
        public int? RepaymentMonths { get; set; }
    }
}