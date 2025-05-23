using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceApi.Models;
using InsuranceApi.Data;

namespace InsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentHistoryEntryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentHistoryEntryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/paymenthistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentHistoryEntry>>> GetPaymentHistoryEntries()
        {
            return await _context.PaymentHistoryEntries.ToListAsync();
        }

        // GET: api/paymenthistory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentHistoryEntry>> GetPaymentHistoryEntry(int id)
        {
            var entry = await _context.PaymentHistoryEntries.FindAsync(id);

            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        // POST: api/paymenthistory
        [HttpPost]
        public async Task<ActionResult<PaymentHistoryEntry>> CreatePaymentHistoryEntry(PaymentHistoryEntry entry)
        {
            _context.PaymentHistoryEntries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaymentHistoryEntry), new { id = entry.Id }, entry);
        }

        // PUT: api/paymenthistory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentHistoryEntry(int id, PaymentHistoryEntry entry)
        {
            if (id != entry.Id)
            {
                return BadRequest();
            }

            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/paymenthistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentHistoryEntry(int id)
        {
            var entry = await _context.PaymentHistoryEntries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.PaymentHistoryEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
