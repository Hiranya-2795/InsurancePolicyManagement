using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceApi.Models
{
    public class PaymentHistory
    {
        [Key]
        public int PaymentID { get; set; } // Primary Key

        [Required]
        [StringLength(50)]
        public string PolicyID { get; set; } // Foreign Key

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public Policy Policy { get; set; } // Navigation Property
    }
}
