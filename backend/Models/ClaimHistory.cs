namespace InsuranceApi.Models
{
    public class ClaimHistory
    {
        public int ClaimHistoryID { get; set; } // Primary Key
        public int ClaimID { get; set; } // Foreign Key
        public DateTime ClaimDate { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal SettledAmount { get; set; }
        public string Status { get; set; }

        public Claim Claim { get; set; }
    }
}
