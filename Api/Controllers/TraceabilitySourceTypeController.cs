using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/traceability-source-types")]
    public class TraceabilitySourceTypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TraceabilitySourceTypeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TraceabilitySourceType>>> GetAllActive()
        {
            var traceabilitySourceTypes = await _context.TraceabilitySourceTypes
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Order)
                .ToListAsync();

            return Ok(traceabilitySourceTypes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TraceabilitySourceType>> GetById(int id)
        {
            var traceabilitySourceType = await _context.TraceabilitySourceTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (traceabilitySourceType == null)
            {
                return NotFound();
            }

            return Ok(traceabilitySourceType);
        }
    }
}