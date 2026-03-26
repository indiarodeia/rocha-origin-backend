using Api.Data;
using Api.Dtos;
using Api.Models;
using Api.Models.Enum;
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

        [HttpPost("search-and-filter/count")]
        public async Task<ActionResult<int>> SearchAndFilterOrdersCount([FromBody] SearchAndFilterOrdersRequestDto request, CancellationToken cancellationToken)
        {
            var search = request.Search?.Trim();
            var statusIds = request.StatusIds?
                .Distinct()
                .ToArray();
            var routeIds = request.RouteIds?
                .Distinct()
                .ToArray();

            IQueryable<Order> query = _context.Orders
                .AsNoTracking()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    (x.Client != null && EF.Functions.ILike(x.Client.CompanyName, $"%{search}%")) ||
                    (x.Establishment != null && EF.Functions.ILike(x.Establishment.Name, $"%{search}%")) ||
                    (x.QuickClientName != null && EF.Functions.ILike(x.QuickClientName, $"%{search}%")) ||
                    _context.OrderItems.Any(oi =>
                        oi.OrderId == x.Id &&
                        (
                            EF.Functions.ILike(oi.ProductName, $"%{search}%") ||
                            (oi.Product != null && EF.Functions.ILike(oi.Product.Name, $"%{search}%")) ||
                            (oi.EstablishmentMenuItem != null && EF.Functions.ILike(oi.EstablishmentMenuItem.Name, $"%{search}%"))
                        )));
            }

            if (statusIds is { Length: > 0 })
            {
                query = query.Where(x => statusIds.Contains(x.OrderStatusId));
            }

            if (routeIds is { Length: > 0 })
            {
                query = query.Where(x => x.RouteId.HasValue && routeIds.Contains(x.RouteId.Value));
            }

            if (request.DeliveryTypeId.HasValue)
            {
                query = query.Where(x => x.DeliveryTypeId == request.DeliveryTypeId.Value);
            }

            if (request.HideDelivered)
            {
                query = query.Where(x => x.OrderStatusId != (int)OrderStatusEnum.Delivered);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            return Ok(totalCount);
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

        [HttpPost("search-and-filter")]
        public async Task<ActionResult<IEnumerable<Order>>> SearchAndFilterOrders([FromBody] SearchAndFilterOrdersRequestDto request, CancellationToken cancellationToken) {
            var search = request.Search?.Trim();
            var statusIds = request.StatusIds?
                .Distinct()
                .ToArray();
            var routeIds = request.RouteIds?
                        .Distinct()
                        .ToArray();

            IQueryable<Order> query = _context.Orders
                .AsNoTracking()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    (x.Client != null && EF.Functions.ILike(x.Client.CompanyName, $"%{search}%")) ||
                    (x.Establishment != null && EF.Functions.ILike(x.Establishment.Name, $"%{search}%")) ||
                    (x.QuickClientName != null && EF.Functions.ILike(x.QuickClientName, $"%{search}%")) ||
                    _context.OrderItems.Any(oi =>
                        oi.OrderId == x.Id &&
                        (
                            EF.Functions.ILike(oi.ProductName, $"%{search}%") ||
                            (oi.Product != null && EF.Functions.ILike(oi.Product.Name, $"%{search}%")) ||
                            (oi.EstablishmentMenuItem != null && EF.Functions.ILike(oi.EstablishmentMenuItem.Name, $"%{search}%"))
                        )));
            }

            if (statusIds is { Length: > 0 })
            {
                query = query.Where(x => statusIds.Contains(x.OrderStatusId));
            }

            if (routeIds is { Length: > 0 })
            {
                query = query.Where(x => x.RouteId.HasValue && routeIds.Contains(x.RouteId.Value));
            }

            if (request.DeliveryTypeId.HasValue)
            {
                query = query.Where(x => x.DeliveryTypeId == request.DeliveryTypeId.Value);
            }

            if (request.HideDelivered)
            {
                query = query.Where(x => x.OrderStatusId != (int)OrderStatusEnum.Delivered);
            }

            query = request.IsSortAscending
                ? query
                    .OrderBy(x => x.DeliveryDate ?? DateTime.MaxValue)
                    .ThenBy(x => x.DeliveryDeadlineTime ?? TimeSpan.MaxValue)
                    .ThenByDescending(x => x.CreatedAt)
                : query
                    .OrderByDescending(x => x.DeliveryDate ?? DateTime.MaxValue)
                    .ThenByDescending(x => x.DeliveryDeadlineTime ?? TimeSpan.MaxValue)
                    .ThenByDescending(x => x.CreatedAt);

            var orders = await query
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
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return Ok(orders);
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

        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<Order>> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequestDto request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (order is null)
            {
                return NotFound();
            }

            if (!Enum.IsDefined(typeof(OrderStatusEnum), request.OrderStatusId))
            {
                return BadRequest("Invalid order status.");
            }

            var status = (OrderStatusEnum)request.OrderStatusId;

            order.OrderStatusId = request.OrderStatusId;
            order.UpdatedAt = DateTime.UtcNow;

            switch (status)
            {
                case OrderStatusEnum.Pending:
                    order.PrepDate = null;
                    order.DeliveryDate = null;
                    order.IsActive = true;
                    break;

                case OrderStatusEnum.Preparing:
                    order.PrepDate ??= DateTime.UtcNow;
                    order.DeliveryDate = null;
                    order.IsActive = true;
                    break;

                case OrderStatusEnum.Ready:
                    order.PrepDate ??= DateTime.UtcNow;
                    order.DeliveryDate = null;
                    order.IsActive = true;
                    break;

                case OrderStatusEnum.Delivered:
                    order.PrepDate ??= DateTime.UtcNow;
                    order.DeliveryDate ??= DateTime.UtcNow;
                    order.IsActive = false;
                    break;
            }

            await _context.SaveChangesAsync(cancellationToken);

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
                .FirstAsync(x => x.Id == id, cancellationToken);

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