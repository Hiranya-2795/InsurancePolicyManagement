using Microsoft.AspNetCore.Mvc;
using InsuranceApi.Data;
using InsuranceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentHistoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentHistory>>> GetPaymentHistories()
        {
            return await _context.PaymentHistories
                                 .Include(ph => ph.Policy) // Eager loading the Policy
                                 .ToListAsync();
        }

        // GET: api/PaymentHistory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentHistory>> GetPaymentHistory(int id)
        {
            var paymentHistory = await _context.PaymentHistories
                                               .Include(ph => ph.Policy)
                                               .FirstOrDefaultAsync(ph => ph.PaymentID == id);

            if (paymentHistory == null)
            {
                return NotFound();
            }

            return paymentHistory;
        }

        // POST: api/PaymentHistory
        [HttpPost]
        public async Task<ActionResult<PaymentHistory>> PostPaymentHistory(PaymentHistory paymentHistory)
        {
            _context.PaymentHistories.Add(paymentHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaymentHistory), new { id = paymentHistory.PaymentID }, paymentHistory);
        }

        // PUT: api/PaymentHistory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentHistory(int id, PaymentHistory paymentHistory)
        {
            if (id != paymentHistory.PaymentID)
            {
                return BadRequest();
            }

            _context.Entry(paymentHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentHistoryExists(id))
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

        // DELETE: api/PaymentHistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentHistory(int id)
        {
            var paymentHistory = await _context.PaymentHistories.FindAsync(id);
            if (paymentHistory == null)
            {
                return NotFound();
            }

            _context.PaymentHistories.Remove(paymentHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentHistoryExists(int id)
        {
            return _context.PaymentHistories.Any(e => e.PaymentID == id);
        }
    }
}
