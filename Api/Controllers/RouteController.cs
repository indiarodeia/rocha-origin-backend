using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Route = Api.Models.Route;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoutesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Route>>> Get()
        {
            var routes = await _context.Routes
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return Ok(routes);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Route>> GetById(Guid id)
        {
            var route = await _context.Routes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (route is null)
            {
                return NotFound();
            }

            return Ok(route);
        }

        [HttpPost]
        public async Task<ActionResult<Route>> Create([FromBody] Route model)
        {
            model.Id = Guid.NewGuid();

            _context.Routes.Add(model);
            await _context.SaveChangesAsync();

            var createdRoute = await _context.Routes
                .AsNoTracking()
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdRoute.Id }, createdRoute);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Route>> Update(Guid id, [FromBody] Route model)
        {
            var route = await _context.Routes.FirstOrDefaultAsync(x => x.Id == id);

            if (route is null)
            {
                return NotFound();
            }

            route.Name = model.Name;
            route.Order = model.Order;
            route.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            var updatedRoute = await _context.Routes
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            return Ok(updatedRoute);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<Route>> SetAsInactive(Guid id)
        {
            var route = await _context.Routes.FirstOrDefaultAsync(x => x.Id == id);

            if (route is null)
            {
                return NotFound();
            }

            route.IsActive = false;

            await _context.SaveChangesAsync();

            var updatedRoute = await _context.Routes
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);

            return Ok(updatedRoute);
        }
    }
}