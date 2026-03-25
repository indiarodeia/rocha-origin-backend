using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("by-order/{orderId:guid}")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetByOrderId(Guid orderId)
        {
            var orderItems = await _context.OrderItems
                .AsNoTracking()
                .Where(x => x.OrderId == orderId && x.Order != null && x.Order.IsActive)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Status)
                .Include(x => x.Order)
                    .ThenInclude(x => x.DeliveryType)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Order)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Product)
                    .ThenInclude(x => x.DefaultUnit)
                .Include(x => x.EstablishmentMenuItem)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.RequestedUnit)
                .Include(x => x.PriceUnit)
                .Include(x => x.PreparedUnit)
                .Include(x => x.TraceabilitySourceType)
                .Include(x => x.Animal)
                .Include(x => x.Lot)
                .Include(x => x.Status)
                .ToListAsync();

            return Ok(orderItems);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderItem>> GetById(Guid id)
        {
            var orderItem = await _context.OrderItems
                .AsNoTracking()
                .Include(x => x.Order)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Status)
                .Include(x => x.Order)
                    .ThenInclude(x => x.DeliveryType)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Order)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Product)
                    .ThenInclude(x => x.DefaultUnit)
                .Include(x => x.EstablishmentMenuItem)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.RequestedUnit)
                .Include(x => x.PriceUnit)
                .Include(x => x.PreparedUnit)
                .Include(x => x.TraceabilitySourceType)
                .Include(x => x.Animal)
                .Include(x => x.Lot)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (orderItem is null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItem>> Create([FromBody] OrderItem model)
        {
            model.Id = Guid.NewGuid();

            _context.OrderItems.Add(model);
            await _context.SaveChangesAsync();

            var createdOrderItem = await _context.OrderItems
                .AsNoTracking()
                .Include(x => x.Order)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Status)
                .Include(x => x.Order)
                    .ThenInclude(x => x.DeliveryType)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Order)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Product)
                    .ThenInclude(x => x.DefaultUnit)
                .Include(x => x.EstablishmentMenuItem)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.RequestedUnit)
                .Include(x => x.PriceUnit)
                .Include(x => x.PreparedUnit)
                .Include(x => x.TraceabilitySourceType)
                .Include(x => x.Animal)
                .Include(x => x.Lot)
                .Include(x => x.Status)
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdOrderItem.Id }, createdOrderItem);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<OrderItem>> Update(Guid id, [FromBody] OrderItem model)
        {
            var orderItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == id);

            if (orderItem is null)
            {
                return NotFound();
            }

            orderItem.OrderId = model.OrderId;
            orderItem.ProductId = model.ProductId;
            orderItem.ProductName = model.ProductName;
            orderItem.EstablishmentMenuItemId = model.EstablishmentMenuItemId;
            orderItem.RequestedQuantity = model.RequestedQuantity;
            orderItem.RequestedUnitId = model.RequestedUnitId;
            orderItem.ApproxKgPerUnit = model.ApproxKgPerUnit;
            orderItem.RequestNotes = model.RequestNotes;
            orderItem.UnitPrice = model.UnitPrice;
            orderItem.PriceUnitId = model.PriceUnitId;
            orderItem.PreparedQuantity = model.PreparedQuantity;
            orderItem.PreparedUnitId = model.PreparedUnitId;
            orderItem.PreparedWeightKg = model.PreparedWeightKg;
            orderItem.TraceabilitySourceTypeId = model.TraceabilitySourceTypeId;
            orderItem.AnimalId = model.AnimalId;
            orderItem.LotId = model.LotId;
            orderItem.OrderStatusId = model.OrderStatusId;
            orderItem.PreparedAt = model.PreparedAt;
            orderItem.PreparedByUserId = model.PreparedByUserId;
            orderItem.PrepNotes = model.PrepNotes;

            await _context.SaveChangesAsync();

            var updatedOrderItem = await _context.OrderItems
                .AsNoTracking()
                .Include(x => x.Order)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Status)
                .Include(x => x.Order)
                    .ThenInclude(x => x.DeliveryType)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Order)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Product)
                    .ThenInclude(x => x.DefaultUnit)
                .Include(x => x.EstablishmentMenuItem)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.RequestedUnit)
                .Include(x => x.PriceUnit)
                .Include(x => x.PreparedUnit)
                .Include(x => x.TraceabilitySourceType)
                .Include(x => x.Animal)
                .Include(x => x.Lot)
                .Include(x => x.Status)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedOrderItem);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<OrderItem>> SetAsInactive(Guid id)
        {
            var orderItem = await _context.OrderItems
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (orderItem is null)
            {
                return NotFound();
            }

            if (orderItem.Order is null)
            {
                return BadRequest("Order item does not have an associated order.");
            }

            orderItem.Order.IsActive = false;
            orderItem.Order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedOrderItem = await _context.OrderItems
                .AsNoTracking()
                .Include(x => x.Order)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Status)
                .Include(x => x.Order)
                    .ThenInclude(x => x.DeliveryType)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Route)
                .Include(x => x.Order)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Product)
                    .ThenInclude(x => x.DefaultUnit)
                .Include(x => x.EstablishmentMenuItem)
                    .ThenInclude(x => x.Establishment)
                .Include(x => x.RequestedUnit)
                .Include(x => x.PriceUnit)
                .Include(x => x.PreparedUnit)
                .Include(x => x.TraceabilitySourceType)
                .Include(x => x.Animal)
                .Include(x => x.Lot)
                .Include(x => x.Status)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedOrderItem);
        }
    }
}