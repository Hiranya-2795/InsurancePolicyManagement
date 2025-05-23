using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InsuranceApi.Models
{
    public class UserPolicy
    {
        [Key, Column(Order = 0)]
        public int UserID { get; set; }

        [Key, Column(Order = 1)]
        [MaxLength(50)]
        public string PolicyID { get; set; }

        public string? BeneficiaryName { get; set; }  // nullable, optional

        [ForeignKey("UserID")]
        public virtual UserProfile? User { get; set; }

        [ForeignKey("PolicyID")]
        public virtual Policy? Policy { get; set; }
    }
}
