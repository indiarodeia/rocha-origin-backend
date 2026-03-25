using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LotsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lot>>> Get()
        {
            var lots = await _context.Lots
                .AsNoTracking()
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ThenByDescending(x => x.WeekOfMonth)
                .ToListAsync();

            return Ok(lots);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Lot>> GetById(Guid id)
        {
            var lot = await _context.Lots
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (lot is null)
            {
                return NotFound();
            }

            return Ok(lot);
        }

        [HttpPost]
        public async Task<ActionResult<Lot>> Create([FromBody] Lot model)
        {
            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTime.UtcNow;

            _context.Lots.Add(model);
            await _context.SaveChangesAsync();

            var createdLot = await _context.Lots
                .AsNoTracking()
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdLot.Id }, createdLot);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Lot>> Update(Guid id, [FromBody] Lot model)
        {
            var lot = await _context.Lots.FirstOrDefaultAsync(x => x.Id == id);

            if (lot is null)
            {
                return NotFound();
            }

            lot.LotCode = model.LotCode;
            lot.Year = model.Year;
            lot.Month = model.Month;
            lot.WeekOfMonth = model.WeekOfMonth;
            lot.StartDate = model.StartDate;
            lot.EndDate = model.EndDate;
            lot.Notes = model.Notes;

            await _context.SaveChangesAsync();

            var updatedLot = await _context.Lots
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            return Ok(updatedLot);
        }
    }
}