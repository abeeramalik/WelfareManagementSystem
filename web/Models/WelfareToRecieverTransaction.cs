using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class WelfareToReceiverTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [ForeignKey("ReceiverRequest")]
        public int RequestId { get; set; }
        public virtual ReceiverRequest Request { get; set; }

        [ForeignKey("UserLoginConfidentials")]
        public int ReceiverId { get; set; }
        public virtual UserLoginConfidentials Receiver { get; set; }

        [ForeignKey("AdminLogin")]
        public int? ApprovedByAdminId { get; set; }
        public virtual AdminLogin ApprovedByAdmin { get; set; }

        [Required]
        public string TransactionType { get; set; } // Shelter, Clothes, Loan, Food

        public decimal MonetaryAmount { get; set; }

        public decimal WelfareBalanceAfter { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        public string Status { get; set; } // Approved, Fulfilled, Completed

        public string Description { get; set; }

        // For Food Transactions
        public int? FoodUnitsProvided { get; set; }

        // For Clothes Transactions
        public int? MaleClothesProvided { get; set; }
        public int? FemaleClothesProvided { get; set; }
        public int? KidsClothesProvided { get; set; }

        // For Shelter Transactions
        public DateTime? ShelterStartDate { get; set; }
        public DateTime? ShelterEndDate { get; set; }
        public int? RoomsAllocated { get; set; }

        // For Loan Transactions - ✅ MADE NULLABLE
        public string? LoanPurpose { get; set; }
        public int? RepaymentMonths { get; set; }
        public decimal? MonthlyRepaymentAmount { get; set; }
    }
}