using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceApi.Data;
using InsuranceApi.Models;
using System.Linq;

namespace InsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminStatisticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminStatisticsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetStatistics()
        {
            // Fetch user profiles excluding admins
            var userCountExcludingAdmins = _context.UserProfiles
                // Assuming you don't have IsAdmin property,
                // filter admins by role or some other property here
                .Count(u => u.Role != "Admin");

            // Total policies in system
            var totalPolicies = _context.Policies.Count();

            // Active user policies count (policies taken by users)
            var activeUserPolicies = _context.UserPolicies.Count();

            // Fetch all policies taken by users joined with policies for grouping
            var userPoliciesWithDetails = (from up in _context.UserPolicies
                                           join p in _context.Policies on up.PolicyID equals p.PolicyID
                                           select p).ToList();

            // Group by PolicyType for the distribution chart
            var policyTypeDistribution = userPoliciesWithDetails
                .GroupBy(p => p.PolicyType)
                .Select(g => new { PolicyType = g.Key, Count = g.Count() })
                .ToList();

            // Group by PremiumFrequency for the premium frequency chart
            var premiumFrequencyDistribution = userPoliciesWithDetails
                .GroupBy(p => p.PremiumFrequency)
                .Select(g => new { PremiumFrequency = g.Key, Count = g.Count() })
                .ToList();

            // Compose response object with counts and distributions
            var response = new
            {
                userCountExcludingAdmins,
                totalPolicies,
                activeUserPolicies,
                policyTypeDistribution,
                premiumFrequencyDistribution
            };

            return Ok(response);
        }
    }
}
