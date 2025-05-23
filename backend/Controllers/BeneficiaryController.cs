using Microsoft.AspNetCore.Mvc;
using InsuranceApi.Data;
using InsuranceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BeneficiaryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Beneficiary
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beneficiary>>> GetBeneficiaries()
        {
            return await _context.Beneficiaries.Include(b => b.Policy).ToListAsync();
        }

        // GET: api/Beneficiary/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Beneficiary>> GetBeneficiary(int id)
        {
            var beneficiary = await _context.Beneficiaries.FindAsync(id);

            if (beneficiary == null)
            {
                return NotFound();
            }

            return beneficiary;
        }

        // POST: api/Beneficiary
        [HttpPost]
        public async Task<ActionResult<Beneficiary>> PostBeneficiary(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Add(beneficiary);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBeneficiary), new { id = beneficiary.BeneficiaryID }, beneficiary);
        }

        // PUT: api/Beneficiary/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeneficiary(int id, Beneficiary beneficiary)
        {
            if (id != beneficiary.BeneficiaryID)
            {
                return BadRequest();
            }

            _context.Entry(beneficiary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeneficiaryExists(id))
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

        // DELETE: api/Beneficiary/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeneficiary(int id)
        {
            var beneficiary = await _context.Beneficiaries.FindAsync(id);
            if (beneficiary == null)
            {
                return NotFound();
            }

            _context.Beneficiaries.Remove(beneficiary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BeneficiaryExists(int id)
        {
            return _context.Beneficiaries.Any(e => e.BeneficiaryID == id);
        }
    }
}
