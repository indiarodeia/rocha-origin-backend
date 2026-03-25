using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/order-statuses")]
    public class OrderStatusesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderStatusesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderStatus>>> GetAllActive()
        {
            var orderStatuses = await _context.OrderStatuses
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Order)
                .ToListAsync();

            return Ok(orderStatuses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderStatus>> GetById(int id)
        {
            var orderStatus = await _context.OrderStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (orderStatus == null)
            {
                return NotFound();
            }

            return Ok(orderStatus);
        }
    }
}