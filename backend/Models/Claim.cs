namespace InsuranceApi.Models
{
    public class Claim
    {
        public int ClaimID { get; set; } // Primary Key
        public string PolicyID { get; set; } // Foreign Key
        public string ClaimStatus { get; set; }
        public decimal ClaimAmount { get; set; }
        public DateTime ClaimDate { get; set; }
        public decimal? SettlementAmount { get; set; }

        public Policy Policy { get; set; }
    }
}
