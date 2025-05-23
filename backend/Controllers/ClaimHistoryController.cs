using Microsoft.AspNetCore.Mvc;
using InsuranceApi.Data;
using InsuranceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClaimHistoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ClaimHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimHistory>>> GetClaimHistories()
        {
            return await _context.ClaimHistories.ToListAsync();
        }

        // GET: api/ClaimHistory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimHistory>> GetClaimHistory(int id)
        {
            var claimHistory = await _context.ClaimHistories.FindAsync(id);

            if (claimHistory == null)
            {
                return NotFound();
            }

            return claimHistory;
        }

        // POST: api/ClaimHistory
        [HttpPost]
        public async Task<ActionResult<ClaimHistory>> PostClaimHistory(ClaimHistory claimHistory)
        {
            _context.ClaimHistories.Add(claimHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClaimHistory), new { id = claimHistory.ClaimHistoryID }, claimHistory);
        }

        // PUT: api/ClaimHistory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClaimHistory(int id, ClaimHistory claimHistory)
        {
            if (id != claimHistory.ClaimHistoryID)
            {
                return BadRequest();
            }

            _context.Entry(claimHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimHistoryExists(id))
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

        // DELETE: api/ClaimHistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaimHistory(int id)
        {
            var claimHistory = await _context.ClaimHistories.FindAsync(id);
            if (claimHistory == null)
            {
                return NotFound();
            }

            _context.ClaimHistories.Remove(claimHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClaimHistoryExists(int id)
        {
            return _context.ClaimHistories.Any(e => e.ClaimHistoryID == id);
        }
    }
}
