using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstablishmentMenuItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstablishmentMenuItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("by-establishment/{establishmentId:guid}")]
        public async Task<ActionResult<IEnumerable<EstablishmentMenuItem>>> GetByEstablishmentId(Guid establishmentId)
        {
            var establishmentMenuItems = await _context.EstablishmentMenuItems
                .AsNoTracking()
                .Where(x => x.IsActive && x.EstablishmentId == establishmentId)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .ToListAsync();

            return Ok(establishmentMenuItems);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EstablishmentMenuItem>> GetById(Guid id)
        {
            var establishmentMenuItem = await _context.EstablishmentMenuItems
                .AsNoTracking()
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (establishmentMenuItem is null)
            {
                return NotFound();
            }

            return Ok(establishmentMenuItem);
        }

        [HttpPost]
        public async Task<ActionResult<EstablishmentMenuItem>> Create([FromBody] EstablishmentMenuItem model)
        {
            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.EstablishmentMenuItems.Add(model);
            await _context.SaveChangesAsync();

            var createdEstablishmentMenuItem = await _context.EstablishmentMenuItems
                .AsNoTracking()
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdEstablishmentMenuItem.Id }, createdEstablishmentMenuItem);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EstablishmentMenuItem>> Update(Guid id, [FromBody] EstablishmentMenuItem model)
        {
            var establishmentMenuItem = await _context.EstablishmentMenuItems.FirstOrDefaultAsync(x => x.Id == id);

            if (establishmentMenuItem is null)
            {
                return NotFound();
            }

            establishmentMenuItem.EstablishmentId = model.EstablishmentId;
            establishmentMenuItem.Name = model.Name;
            establishmentMenuItem.Description = model.Description;
            establishmentMenuItem.IsActive = model.IsActive;
            establishmentMenuItem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedEstablishmentMenuItem = await _context.EstablishmentMenuItems
                .AsNoTracking()
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedEstablishmentMenuItem);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<EstablishmentMenuItem>> SetAsInactive(Guid id)
        {
            var establishmentMenuItem = await _context.EstablishmentMenuItems.FirstOrDefaultAsync(x => x.Id == id);

            if (establishmentMenuItem is null)
            {
                return NotFound();
            }

            establishmentMenuItem.IsActive = false;
            establishmentMenuItem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedEstablishmentMenuItem = await _context.EstablishmentMenuItems
                .AsNoTracking()
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Client)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.DeliveryAddress)
                .Include(x => x.Establishment)
                    .ThenInclude(x => x.Route)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedEstablishmentMenuItem);
        }
    }
}