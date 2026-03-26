using Api.Data;
using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnimalsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> Get()
        {
            var animals = await _context.Animals
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Include(x => x.Species)
                .Include(x => x.Supplier)
                .OrderByDescending(x => x.SlaughterDate)
                .ToListAsync();

            return Ok(animals);
        }

        [HttpGet("by-supplier/{supplierId:guid}")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetBySupplier(Guid supplierId)
        {
            var animals = await _context.Animals
                .AsNoTracking()
                .Where(x => x.IsActive && x.SupplierId == supplierId)
                .Include(x => x.Species)
                .Include(x => x.Supplier)
                .OrderByDescending(x => x.SlaughterDate)
                .ToListAsync();

            return Ok(animals);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Animal>> GetById(Guid id)
        {
            var animal = await _context.Animals
                .AsNoTracking()
                .Include(x => x.Species)
                .Include(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (animal is null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        [HttpPost("search-and-filter")]
        public async Task<ActionResult<IEnumerable<Animal>>> SearchAndFilterAnimals([FromBody] SearchAndFilterAnimalsRequestDto request, CancellationToken cancellationToken)
        {
            var search = request.Search?.Trim();

            IQueryable<Animal> query = _context.Animals
                .AsNoTracking()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    EF.Functions.ILike(x.AnimalIdentification, $"%{search}%") ||
                    (x.Supplier != null && EF.Functions.ILike(x.Supplier.Name, $"%{search}%")) ||
                    (x.Breed != null && EF.Functions.ILike(x.Breed, $"%{search}%")) ||
                    (x.EuropRaw != null && EF.Functions.ILike(x.EuropRaw, $"%{search}%")));
            }

            if (request.SpeciesId.HasValue)
            {
                query = query.Where(x => x.AnimalSpeciesId == request.SpeciesId.Value);
            }

            if (request.SupplierId.HasValue)
            {
                query = query.Where(x => x.SupplierId == request.SupplierId.Value);
            }

            query = request.SortBy?.Trim().ToLowerInvariant() switch
            {
                "arrivaldate" => request.IsSortAscending
                    ? query.OrderBy(x => x.ArrivalDate ?? DateTime.MaxValue)
                        .ThenBy(x => x.AnimalIdentification)
                    : query.OrderByDescending(x => x.ArrivalDate ?? DateTime.MinValue)
                        .ThenBy(x => x.AnimalIdentification),

                "identification" => request.IsSortAscending
                    ? query.OrderBy(x => x.AnimalIdentification)
                        .ThenByDescending(x => x.SlaughterDate)
                    : query.OrderByDescending(x => x.AnimalIdentification)
                        .ThenByDescending(x => x.SlaughterDate),

                _ => request.IsSortAscending
                    ? query.OrderBy(x => x.SlaughterDate)
                        .ThenBy(x => x.AnimalIdentification)
                    : query.OrderByDescending(x => x.SlaughterDate)
                        .ThenBy(x => x.AnimalIdentification)
            };

            var animals = await query
                .Include(x => x.Species)
                .Include(x => x.Supplier)
                .ThenInclude(x => x.Address)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return Ok(animals);
        }

        [HttpPost("search-and-filter/count")]
        public async Task<ActionResult<int>> SearchAndFilterAnimalsCount([FromBody] SearchAndFilterAnimalsRequestDto request, CancellationToken cancellationToken)
        {
            var search = request.Search?.Trim();

            IQueryable<Animal> query = _context.Animals
                .AsNoTracking()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    EF.Functions.ILike(x.AnimalIdentification, $"%{search}%") ||
                    (x.Supplier != null && EF.Functions.ILike(x.Supplier.Name, $"%{search}%")) ||
                    (x.Breed != null && EF.Functions.ILike(x.Breed, $"%{search}%")) ||
                    (x.EuropRaw != null && EF.Functions.ILike(x.EuropRaw, $"%{search}%")));
            }

            if (request.SpeciesId.HasValue)
            {
                query = query.Where(x => x.AnimalSpeciesId == request.SpeciesId.Value);
            }

            if (request.SupplierId.HasValue)
            {
                query = query.Where(x => x.SupplierId == request.SupplierId.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            return Ok(totalCount);
        }

        [HttpPost]
        public async Task<ActionResult<Animal>> Create([FromBody] Animal model)
        {
            model.Id = Guid.NewGuid();

            _context.Animals.Add(model);
            await _context.SaveChangesAsync();

            var createdAnimal = await _context.Animals
                .AsNoTracking()
                .Include(x => x.Species)
                .Include(x => x.Supplier)
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdAnimal.Id }, createdAnimal);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Animal>> Update(Guid id, [FromBody] Animal model)
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == id);

            if (animal is null)
            {
                return NotFound();
            }

            animal.AnimalSpeciesId = model.AnimalSpeciesId;
            animal.AnimalIdentification = model.AnimalIdentification;
            animal.SlaughterDate = model.SlaughterDate;
            animal.DispatchDate = model.DispatchDate;
            animal.ArrivalDate = model.ArrivalDate;
            animal.BirthPlace = model.BirthPlace;
            animal.RearingPlace = model.RearingPlace;
            animal.Breed = model.Breed;
            animal.SupplierId = model.SupplierId;
            animal.ColdWeightKg = model.ColdWeightKg;
            animal.AgeMonths = model.AgeMonths;
            animal.Ph = model.Ph;
            animal.EuropConformation = model.EuropConformation;
            animal.EuropFatClass = model.EuropFatClass;
            animal.EuropCategory = model.EuropCategory;
            animal.EuropRaw = model.EuropRaw;
            animal.CarcassPhotoUrl = model.CarcassPhotoUrl;
            animal.SlaughterhouseRef = model.SlaughterhouseRef;
            animal.Notes = model.Notes;
            animal.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            var updatedAnimal = await _context.Animals
                .AsNoTracking()
                .Include(x => x.Species)
                .Include(x => x.Supplier)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedAnimal);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<Animal>> SetAsInactive(Guid id)
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(x => x.Id == id);

            if (animal is null)
            {
                return NotFound();
            }

            animal.IsActive = false;

            await _context.SaveChangesAsync();

            var updatedAnimal = await _context.Animals
                .AsNoTracking()
                .Include(x => x.Species)
                .Include(x => x.Supplier)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedAnimal);
        }
    }
}