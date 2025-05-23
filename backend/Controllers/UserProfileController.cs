using Microsoft.AspNetCore.Mvc;
using InsuranceApi.Data;
using InsuranceApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace InsuranceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(AppDbContext context, ILogger<UserProfileController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/UserProfile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUserProfiles()
        {
            var users = await _context.UserProfiles
                .Select(u => new
                {
                    u.UserID,
                    u.FullName,
                    u.DateOfBirth,
                    u.Gender,
                    u.PhoneNumber,
                    u.Email,
                    u.AadharNumber,
                    u.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/UserProfile/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUserProfile(int id)
        {
            var user = await _context.UserProfiles.FindAsync(id);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.UserID,
                user.FullName,
                user.DateOfBirth,
                user.Gender,
                user.PhoneNumber,
                user.Email,
                user.AadharNumber,
                user.Role
            });
        }

        // POST: api/UserProfile
        [HttpPost]
        public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfile userProfile)
        {
            if (string.IsNullOrWhiteSpace(userProfile.Password))
                return BadRequest("Password is required.");

            userProfile.PasswordHash = HashPassword(userProfile.Password);

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserProfile), new { id = userProfile.UserID }, userProfile);
        }

        // PUT: api/UserProfile/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(int id, UserProfile userProfile)
        {
            var existingUser = await _context.UserProfiles.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            // Update fields
            existingUser.FullName = userProfile.FullName ?? existingUser.FullName;
            existingUser.DateOfBirth = userProfile.DateOfBirth ?? existingUser.DateOfBirth;
            existingUser.Gender = userProfile.Gender ?? existingUser.Gender;
            existingUser.PhoneNumber = userProfile.PhoneNumber ?? existingUser.PhoneNumber;
            existingUser.Email = userProfile.Email ?? existingUser.Email;
            existingUser.AadharNumber = userProfile.AadharNumber ?? existingUser.AadharNumber;
            existingUser.Role = userProfile.Role ?? existingUser.Role;

            if (!string.IsNullOrWhiteSpace(userProfile.Password))
            {
                existingUser.PasswordHash = HashPassword(userProfile.Password);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/UserProfile/{userId}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            var user = await _context.UserProfiles.FindAsync(id);
            if (user == null)
                return NotFound();

            var hasPolicies = _context.UserPolicies.Any(p => p.UserID == id);
            if (hasPolicies)
                return BadRequest("User has associated policies.");

            _context.UserProfiles.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    }
}
