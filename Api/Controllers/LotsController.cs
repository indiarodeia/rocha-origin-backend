using Api.Data;
using Api.Dtos;
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

        [HttpPost("{lotId:guid}/animals")]
        public async Task<ActionResult<Lot>> AddAnimalToLot(Guid lotId, [FromBody] AddAnimalToLotRequestDto request, CancellationToken cancellationToken)
        {
            if (request.AnimalIds is not { Count: > 0 })
            {
                return BadRequest("At least one animalId must be provided.");
            }

            var animalIds = request.AnimalIds
                .Where(x => x != Guid.Empty)
                .Distinct()
                .ToArray();

            if (animalIds.Length == 0)
            {
                return BadRequest("At least one valid animalId must be provided.");
            }

            var lotExists = await _context.Lots
                .AsNoTracking()
                .AnyAsync(x => x.Id == lotId, cancellationToken);

            if (!lotExists)
            {
                return NotFound();
            }

            var existingAnimalCount = await _context.Animals
                .AsNoTracking()
                .CountAsync(x => animalIds.Contains(x.Id), cancellationToken);

            if (existingAnimalCount != animalIds.Length)
            {
                return BadRequest("One or more animalIds are invalid.");
            }

            var alreadyLinkedAnimalIds = await _context.Set<LotAnimal>()
                .AsNoTracking()
                .Where(x => x.LotId == lotId && animalIds.Contains(x.AnimalId))
                .Select(x => x.AnimalId)
                .ToListAsync(cancellationToken);

            var utcNow = DateTime.UtcNow;

            var lotAnimalsToAdd = animalIds
                .Except(alreadyLinkedAnimalIds)
                .Select(animalId => new LotAnimal
                {
                    LotId = lotId,
                    AnimalId = animalId,
                    AddedAt = utcNow
                })
                .ToList();

            if (lotAnimalsToAdd.Count > 0)
            {
                await _context.Set<LotAnimal>().AddRangeAsync(lotAnimalsToAdd, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var updatedLot = await _context.Lots
                .AsNoTracking()
                .FirstAsync(x => x.Id == lotId, cancellationToken);

            return Ok(updatedLot);
        }

        [HttpDelete("{lotId:guid}/animals")]
        public async Task<ActionResult<Lot>> RemoveAnimalFromLot(Guid lotId, [FromBody] RemoveAnimalFromLotRequestDto request, CancellationToken cancellationToken)
        {
            if (request.AnimalIds is not { Count: > 0 })
            {
                return BadRequest("At least one animalId must be provided.");
            }

            var animalIds = request.AnimalIds
                .Where(x => x != Guid.Empty)
                .Distinct()
                .ToArray();

            if (animalIds.Length == 0)
            {
                return BadRequest("At least one valid animalId must be provided.");
            }

            var lotExists = await _context.Lots
                .AsNoTracking()
                .AnyAsync(x => x.Id == lotId, cancellationToken);

            if (!lotExists)
            {
                return NotFound();
            }

            await _context.Set<LotAnimal>()
                .Where(x => x.LotId == lotId && animalIds.Contains(x.AnimalId))
                .ExecuteDeleteAsync(cancellationToken);

            var updatedLot = await _context.Lots
                .AsNoTracking()
                .FirstAsync(x => x.Id == lotId, cancellationToken);

            return Ok(updatedLot);
        }
    }
}