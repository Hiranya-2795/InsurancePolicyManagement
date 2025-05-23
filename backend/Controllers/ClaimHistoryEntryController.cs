using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceApi.Models;
using InsuranceApi.Data;

namespace InsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaimHistoryEntryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClaimHistoryEntryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/claimhistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimHistoryEntry>>> GetClaimHistoryEntries()
        {
            return await _context.ClaimHistoryEntries.ToListAsync();
        }

        // GET: api/claimhistory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimHistoryEntry>> GetClaimHistoryEntry(int id)
        {
            var entry = await _context.ClaimHistoryEntries.FindAsync(id);

            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        // POST: api/claimhistory
        [HttpPost]
        public async Task<ActionResult<ClaimHistoryEntry>> CreateClaimHistoryEntry(ClaimHistoryEntry entry)
        {
            _context.ClaimHistoryEntries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClaimHistoryEntry), new { id = entry.Id }, entry);
        }

        // PUT: api/claimhistory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClaimHistoryEntry(int id, ClaimHistoryEntry entry)
        {
            if (id != entry.Id)
            {
                return BadRequest();
            }

            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/claimhistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClaimHistoryEntry(int id)
        {
            var entry = await _context.ClaimHistoryEntries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.ClaimHistoryEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
