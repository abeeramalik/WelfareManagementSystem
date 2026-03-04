using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class AdminLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AdminId { get; set; }

        [Required]
        public string passwordHash { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        // Navigation properties
        public virtual ICollection<WelfareToNGORequest> NGORequests { get; set; }
        public virtual ICollection<WelfareToReceiverTransaction> ReceiverApprovals { get; set; }
    }
}