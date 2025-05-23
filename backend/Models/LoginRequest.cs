namespace InsuranceApi.Models
{
    public class LoginRequest
    {
        public string Email { get; set; }  // or Username if that's what you use
        public string Password { get; set; }
    }
}
