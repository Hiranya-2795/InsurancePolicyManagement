using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InsuranceApi.Models
{
    public class Policy
    {
        [Key]
        [StringLength(50)]
        public string PolicyID { get; set; }  // nvarchar(50) in DB

        [Required]
        public string PolicyType { get; set; }  // nvarchar in DB

        [Required]
        public DateTime StartDate { get; set; }  // date in DB

        [Required]
        public DateTime EndDate { get; set; }  // date in DB

        [Required]
        public int PolicyTerm { get; set; }  // int in DB


        [Required]
        public decimal CoverageAmount { get; set; }  // decimal in DB

        [Required]
        public decimal PremiumAmount { get; set; }  // decimal in DB

        [Required]
        public string PremiumFrequency { get; set; }  // nvarchar in DB

        // Navigation property for related PaymentHistory records
        // public ICollection<PaymentHistory> PaymentHistories { get; set; }
    }
}
