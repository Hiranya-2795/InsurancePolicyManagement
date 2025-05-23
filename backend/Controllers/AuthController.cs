using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using InsuranceApi.Data;
using InsuranceApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InsuranceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Register a new user
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.UserProfiles.AnyAsync(u => u.Email == registerRequest.Email))
                return BadRequest("User with this email already exists.");

            var user = new UserProfile
            {
                FullName = registerRequest.FullName,
                DateOfBirth = registerRequest.DateOfBirth,
                Gender = registerRequest.Gender,
                PhoneNumber = registerRequest.PhoneNumber,
                Email = registerRequest.Email,
                AadharNumber = registerRequest.AadharNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                Role = "User" // default role
            };

            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }


        // Login existing user
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.UserProfiles.SingleOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null)
                return NotFound("Email not found.");

            if (string.IsNullOrEmpty(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
                return Unauthorized("Incorrect password.");

            if (!string.Equals(user.Role, loginRequest.Role, StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Role mismatch.");

            var token = GenerateJwtToken(user);

            return Ok(new { Token = token, Role = user.Role });
        }

        // JWT generation
        private string GenerateJwtToken(UserProfile user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMonths(1);

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // DTOs
    public class RegisterRequest
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required, MaxLength(10)]
        public string Gender { get; set; }

        [Required, StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(12, MinimumLength = 12)]
        public string AadharNumber { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // Admin/User
    }

    public class LoginRequest
    {
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
