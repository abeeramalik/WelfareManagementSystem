using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class FoodDistribution
    {
        [Key]
        public int DistributionId { get; set; }

        [ForeignKey("ReceiverRequest")]
        public int RequestId { get; set; }
        public virtual ReceiverRequest Request { get; set; }

        public int FamilyMembers { get; set; }
        public int FoodQuantityGiven { get; set; } // Number of ration units given
        public string FoodUnit { get; set; } // rations, kg, packages

        public decimal TotalCost { get; set; }

        public DateTime DistributionDate { get; set; }
        public string DistributedBy { get; set; } // Admin name or ID
    }
}