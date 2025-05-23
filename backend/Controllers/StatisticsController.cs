using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsuranceApi.Models;
using System.Linq;
using InsuranceApi.Data;

namespace InsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/statistics
        [HttpGet]
        public ActionResult<StatisticsDto> GetStatistics()
        {
            var statistics = new StatisticsDto
            {
                NumOfCustomers = _context.Users.Count(),
                TotalPolicies = _context.Policies.Count(),
                ActivePolicies = _context.Policies.Count(p => p.ClaimStatus == "Active"),
                TotalClaims = _context.ClaimHistoryEntries.Count(),
                ActiveClaims = _context.ClaimHistoryEntries.Count(c => c.Status == "Active"),
                PendingClaims = _context.ClaimHistoryEntries.Count(c => c.Status == "Pending")
            };

            return Ok(statistics);
        }
    }
}
