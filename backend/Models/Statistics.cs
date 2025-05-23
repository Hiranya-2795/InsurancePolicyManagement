namespace InsuranceApi.Models
{
    public class Statistics
    {
        public int UserCountExcludingAdmins { get; set; }
        public int TotalPolicies { get; set; }
        public int ActiveUserPolicies { get; set; }

        // New properties for charts
        public Dictionary<string, int> PolicyTypeDistribution { get; set; }
        public Dictionary<string, int> PremiumFrequencyDistribution { get; set; }
    }
}
