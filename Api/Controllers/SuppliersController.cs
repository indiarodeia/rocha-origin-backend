using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuppliersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> Get()
        {
            var suppliers = await _context.Suppliers
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Include(x => x.Address)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Ok(suppliers);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Supplier>> GetById(Guid id)
        {
            var supplier = await _context.Suppliers
                .AsNoTracking()
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supplier is null)
            {
                return NotFound();
            }

            return Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult<Supplier>> Create([FromBody] Supplier model)
        {
            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTime.UtcNow;
            model.UpdateAt = DateTime.UtcNow;

            _context.Suppliers.Add(model);
            await _context.SaveChangesAsync();

            var createdSupplier = await _context.Suppliers
                .AsNoTracking()
                .Include(x => x.Address)
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdSupplier.Id }, createdSupplier);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Supplier>> Update(Guid id, [FromBody] Supplier model)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id);

            if (supplier is null)
            {
                return NotFound();
            }

            supplier.Name = model.Name;
            supplier.VatNumber = model.VatNumber;
            supplier.VatRate = model.VatRate;
            supplier.ExplorationId = model.ExplorationId;
            supplier.AddressId = model.AddressId;
            supplier.ProfilePicture = model.ProfilePicture;
            supplier.CoverPicture = model.CoverPicture;
            supplier.Phone = model.Phone;
            supplier.Email = model.Email;
            supplier.Certifications = model.Certifications;
            supplier.IsActive = model.IsActive;
            supplier.UpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedSupplier = await _context.Suppliers
                .AsNoTracking()
                .Include(x => x.Address)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedSupplier);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<Supplier>> SetAsInactive(Guid id)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.Id == id);

            if (supplier is null)
            {
                return NotFound();
            }

            supplier.IsActive = false;
            supplier.UpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedSupplier = await _context.Suppliers
                .AsNoTracking()
                .Include(x => x.Address)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedSupplier);
        }
    }
}