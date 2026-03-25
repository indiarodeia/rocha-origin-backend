using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstablishmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstablishmentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Establishment>>> Get()
        {
            var establishments = await _context.Establishments
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.DeliveryAddress)
                .Include(x => x.Route)
                .Include(x => x.EstablishmentMenuItems)
                .ToListAsync();

            return Ok(establishments);
        }

        [HttpGet("by-client/{clientId:guid}")]
        public async Task<ActionResult<IEnumerable<Establishment>>> GetByClient(Guid clientId)
        {
            var establishments = await _context.Establishments
                .AsNoTracking()
                .Where(x => x.IsActive && x.ClientId == clientId)
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.DeliveryAddress)
                .Include(x => x.Route)
                .Include(x => x.EstablishmentMenuItems)
                .ToListAsync();

            return Ok(establishments);
        }

        [HttpGet("by-route/{routeId:guid}")]
        public async Task<ActionResult<IEnumerable<Establishment>>> GetByRoute(Guid routeId)
        {
            var establishments = await _context.Establishments
                .AsNoTracking()
                .Where(x => x.IsActive && x.RouteId == routeId)
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.DeliveryAddress)
                .Include(x => x.Route)
                .Include(x => x.EstablishmentMenuItems)
                .ToListAsync();

            return Ok(establishments);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Establishment>> GetById(Guid id)
        {
            var establishment = await _context.Establishments
                .AsNoTracking()
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.DeliveryAddress)
                .Include(x => x.Route)
                .Include(x => x.EstablishmentMenuItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (establishment is null)
            {
                return NotFound();
            }

            return Ok(establishment);
        }

        [HttpPost]
        public async Task<ActionResult<Establishment>> Create([FromBody] Establishment model)
        {
            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Establishments.Add(model);
            await _context.SaveChangesAsync();

            var createdEstablishment = await _context.Establishments
                .AsNoTracking()
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.DeliveryAddress)
                .Include(x => x.Route)
                .Include(x => x.EstablishmentMenuItems)
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdEstablishment.Id }, createdEstablishment);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Establishment>> Update(Guid id, [FromBody] Establishment model)
        {
            var establishment = await _context.Establishments.FirstOrDefaultAsync(x => x.Id == id);

            if (establishment is null)
            {
                return NotFound();
            }

            establishment.ClientId = model.ClientId;
            establishment.Name = model.Name;
            establishment.IsActive = model.IsActive;
            establishment.DeliveryAddressId = model.DeliveryAddressId;
            establishment.RouteId = model.RouteId;
            establishment.LocalContactPhone = model.LocalContactPhone;
            establishment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedEstablishment = await _context.Establishments
                .AsNoTracking()
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.DeliveryAddress)
                .Include(x => x.Route)
                .Include(x => x.EstablishmentMenuItems)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedEstablishment);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<Establishment>> SetAsInactive(Guid id)
        {
            var establishment = await _context.Establishments.FirstOrDefaultAsync(x => x.Id == id);

            if (establishment is null)
            {
                return NotFound();
            }

            establishment.IsActive = false;
            establishment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedEstablishment = await _context.Establishments
                .AsNoTracking()
                .Include(x => x.Client)
                    .ThenInclude(x => x.BillingAddress)
                .Include(x => x.Client)
                    .ThenInclude(x => x.PaymentType)
                .Include(x => x.DeliveryAddress)
                .Include(x => x.Route)
                .Include(x => x.EstablishmentMenuItems)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedEstablishment);
        }
    }
}