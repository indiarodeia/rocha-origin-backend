using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get()
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Status)
                .Include(x => x.DeliveryType)
                .Include(x => x.Route)
                .Include(x => x.PaymentType)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("by-client/{clientId:guid}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetByClient(Guid clientId)
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Where(x => x.IsActive && x.ClientId == clientId)
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Status)
                .Include(x => x.DeliveryType)
                .Include(x => x.Route)
                .Include(x => x.PaymentType)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("by-establishment/{establishmentId:guid}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetByEstablishment(Guid establishmentId)
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Where(x => x.IsActive && x.EstablishmentId == establishmentId)
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Status)
                .Include(x => x.DeliveryType)
                .Include(x => x.Route)
                .Include(x => x.PaymentType)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> GetById(Guid id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Status)
                .Include(x => x.DeliveryType)
                .Include(x => x.Route)
                .Include(x => x.PaymentType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order is null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] Order model)
        {
            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Orders.Add(model);
            await _context.SaveChangesAsync();

            var createdOrder = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Status)
                .Include(x => x.DeliveryType)
                .Include(x => x.Route)
                .Include(x => x.PaymentType)
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Order>> Update(Guid id, [FromBody] Order model)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (order is null)
            {
                return NotFound();
            }

            order.ClientId = model.ClientId;
            order.EstablishmentId = model.EstablishmentId;
            order.QuickClientName = model.QuickClientName;
            order.OrderStatusId = model.OrderStatusId;
            order.PrepDate = model.PrepDate;
            order.DeliveryDate = model.DeliveryDate;
            order.DeliveryDeadlineTime = model.DeliveryDeadlineTime;
            order.DeliveryTypeId = model.DeliveryTypeId;
            order.IsUrgent = model.IsUrgent;
            order.OrderCategory = model.OrderCategory;
            order.RouteId = model.RouteId;
            order.PaymentTypeId = model.PaymentTypeId;
            order.Notes = model.Notes;
            order.CreatedByUserId = model.CreatedByUserId;
            order.IsActive = model.IsActive;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedOrder = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Status)
                .Include(x => x.DeliveryType)
                .Include(x => x.Route)
                .Include(x => x.PaymentType)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedOrder);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<Order>> SetAsInactive(Guid id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (order is null)
            {
                return NotFound();
            }

            order.IsActive = false;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedOrder = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Status)
                .Include(x => x.DeliveryType)
                .Include(x => x.Route)
                .Include(x => x.PaymentType)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedOrder);
        }
    }
}