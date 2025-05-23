using InsuranceApi.Models;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public DateTime Dob { get; set; }

    [Required]
    public string Gender { get; set; }

    [Required]
    [Phone]
    [RegularExpression(@"^\d{10}$")]
    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^\d{12}$")]
    public string AadharNo { get; set; }

    // 🔐 Replace this:
    // public string Password { get; set; }

    // ✅ Add secure password fields:
    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Salt { get; set; }

    [Required]
    public string Role { get; set; } // "ADMIN" or "CUSTOMER"

    public ICollection<Policy> Policies { get; set; }
}
