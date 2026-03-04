using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web.Models
{
    public class UserLoginConfidentials
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(100000, 999999, ErrorMessage = "UserId must be a 6-digit number.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "User Type is required")]
        [StringLength(20)]
        public string UserType { get; set; } // "Donor" or "Receiver"

        [Required(ErrorMessage = "CNIC is required")]
        [StringLength(15)]
        [RegularExpression(@"^\d{5}-\d{7}-\d{1}$", ErrorMessage = "CNIC format should be XXXXX-XXXXXXX-X")]
        public string CNIC { get; set; } // Unique

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; } // Unique

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone format")]
        [StringLength(12)]
        [RegularExpression(@"^03\d{2}-\d{7}$", ErrorMessage = "Phone format should be 03XX-XXXXXXX")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(20)]
        public string Gender { get; set; } // Male, Female, Other, PreferNotToSay

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50)]
        public string City { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // Navigation properties - Not required for registration
        public virtual ICollection<ReceiverRequest>? ReceiverRequests { get; set; }
        public virtual ICollection<DonorToWelfareTransaction>? DonorTransactions { get; set; }
    }
}