using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/payment-types")]
    public class PaymentTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PaymentTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentType>>> GetAllActive(CancellationToken cancellationToken)
        {
            var paymentTypes = await _context.PaymentTypes
                .AsNoTracking()
                .Where(p => p.IsActive)
                .OrderBy(p => p.Order)
                .ToListAsync(cancellationToken);

            return Ok(paymentTypes);
        }

        // GET: api/payment-types/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaymentType>> GetById(int id, CancellationToken cancellationToken)
        {
            var paymentType = await _context.PaymentTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (paymentType == null)
                return NotFound();

            return Ok(paymentType);
        }
    }
}