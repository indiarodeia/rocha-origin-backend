using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/animal-species")]
    public class AnimalSpeciesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnimalSpeciesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnimalSpecies>>> GetAllActive()
        {
            var animalSpecies = await _context.AnimalSpecies
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Order)
                .ToListAsync();

            return Ok(animalSpecies);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AnimalSpecies>> GetById(int id)
        {
            var animalSpecies = await _context.AnimalSpecies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (animalSpecies == null)
            {
                return NotFound();
            }

            return Ok(animalSpecies);
        }
    }
}