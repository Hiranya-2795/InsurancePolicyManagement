using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceApi.Models;
using InsuranceApi.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PolicyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Policy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Policy>>> GetPolicies()
        {
            return await _context.Policies.ToListAsync();
        }

        // GET: api/Policy/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Policy>> GetPolicy(string id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }
            return policy;
        }

        // POST: api/Policy
        [HttpPost]
        public async Task<ActionResult<Policy>> PostPolicy(Policy policy)
        {
            if (policy == null)
            {
                return BadRequest("Policy data is required.");
            }

            _context.Policies.Add(policy);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Internal server error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return CreatedAtAction(nameof(GetPolicy), new { id = policy.PolicyID }, policy);
        }

        // PUT: api/Policy/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPolicy(string id, Policy policy)
        {
            if (id != policy.PolicyID)
            {
                return BadRequest("Policy ID mismatch.");
            }

            _context.Entry(policy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolicyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Policy/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicy(string id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // SEARCH: api/Policy/search?policyId=&policyType=&premiumFrequency=
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Policy>>> SearchPolicies(
            [FromQuery] string? policyId,
            [FromQuery] string? policyType,
            [FromQuery] string? premiumFrequency)
        {
            var query = _context.Policies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(policyId))
            {
                query = query.Where(p => p.PolicyID.ToLower().Contains(policyId.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(policyType))
            {
                query = query.Where(p => p.PolicyType != null &&
                                         p.PolicyType.ToLower() == policyType.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(premiumFrequency))
            {
                query = query.Where(p => p.PremiumFrequency != null &&
                                         p.PremiumFrequency.ToLower() == premiumFrequency.ToLower());
            }

            var result = await query.ToListAsync();

            if (result.Count == 0)
                return NotFound("No policies found matching the criteria.");

            return result;
        }

        private bool PolicyExists(string id)
        {
            return _context.Policies.Any(e => e.PolicyID == id);
        }
    }
}
