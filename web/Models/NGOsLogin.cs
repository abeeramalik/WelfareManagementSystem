using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class NGOsLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(100000, 999999, ErrorMessage = "NgoId must be a 6-digit number.")]
        public int NgoId { get; set; }

        [Required]
        [StringLength(200)]
        public string OrganizationName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Unique

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        public string RegistrationNumber { get; set; } // NGO registration certificate number

        public DateTime RegistrationDate { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsVerified { get; set; } = false; // Admin verification status

        // Navigation properties
        public virtual ICollection<NGOToWelfareTransaction> NGOTransactions { get; set; }
        public virtual ICollection<WelfareToNGORequest> WelfareRequests { get; set; }
    }
}