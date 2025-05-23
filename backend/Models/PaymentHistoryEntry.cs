using System.ComponentModel.DataAnnotations;

namespace InsuranceApi.Models
{
    public class PaymentHistoryEntry
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        // Foreign Key
        public int PolicyId { get; set; }
        public Policy Policy { get; set; }
    }
}
