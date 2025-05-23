using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceApi.Models
{
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required, MaxLength(10)]
        public string Gender { get; set; }

        [Required, StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, StringLength(12)]
        public string AadharNumber { get; set; }

        public string? PasswordHash { get; set; }

        [Required, MaxLength(10)]
        public string Role { get; set; }

        [NotMapped]
        public string Password { get; set; }
    }
}
