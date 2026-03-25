using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AddressesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> Get()
        {
            var addresses = await _context.Addresses
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();

            return Ok(addresses);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Address>> GetById(Guid id)
        {
            var address = await _context.Addresses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (address is null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult<Address>> Create([FromBody] Address model)
        {
            model.Id = Guid.NewGuid();

            _context.Addresses.Add(model);
            await _context.SaveChangesAsync();

            var createdAddress = await _context.Addresses
                .AsNoTracking()
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdAddress.Id }, createdAddress);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Address>> Update(Guid id, [FromBody] Address model)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

            if (address is null)
            {
                return NotFound();
            }

            address.Street = model.Street;
            address.DoorNumber = model.DoorNumber;
            address.PostalCode = model.PostalCode;
            address.City = model.City;
            address.Country = model.Country;
            address.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            var updatedAddress = await _context.Addresses
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            return Ok(updatedAddress);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<Address>> SetAsInactive(Guid id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

            if (address is null)
            {
                return NotFound();
            }

            address.IsActive = false;

            await _context.SaveChangesAsync();

            var updatedAddress = await _context.Addresses
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            return Ok(updatedAddress);
        }
    }
}