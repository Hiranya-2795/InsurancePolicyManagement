using InsuranceApi.Models;

public class Beneficiary
{
    public int BeneficiaryID { get; set; }
    public int PolicyID { get; set; }
    public string BeneficiaryName { get; set; }

    // Navigation Property
    public virtual Policy Policy { get; set; }
}
