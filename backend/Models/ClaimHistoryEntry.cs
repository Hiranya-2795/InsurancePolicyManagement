using System.ComponentModel.DataAnnotations;

namespace InsuranceApi.Models
{
    public class ClaimHistoryEntry
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        // Foreign Key
        public int PolicyId { get; set; }
        public Policy Policy { get; set; }
    }
}
