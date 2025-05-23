namespace InsuranceApi.Models
{
    public class StatisticsDto
    {
        public int NumOfCustomers { get; set; }
        public int TotalPolicies { get; set; }
        public int ActivePolicies { get; set; }
        public int TotalClaims { get; set; }
        public int ActiveClaims { get; set; }
        public int PendingClaims { get; set; }
    }
}
