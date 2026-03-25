using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/delivery-types")]
    public class DeliveryTypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeliveryTypeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryType>>> GetAllActive()
        {
            var deliveryTypes = await _context.DeliveryTypes
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Order)
                .ToListAsync();

            return Ok(deliveryTypes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DeliveryType>> GetById(int id)
        {
            var deliveryType = await _context.DeliveryTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (deliveryType == null)
            {
                return NotFound();
            }

            return Ok(deliveryType);
        }
    }
}