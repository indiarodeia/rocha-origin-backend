using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/product-units")]
    public class ProductUnitController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductUnitController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductUnit>>> GetAllActive()
        {
            var productUnits = await _context.ProductUnits
                .AsNoTracking()
                .Where(p => p.IsActive)
                .OrderBy(p => p.Order)
                .ToListAsync();

            return Ok(productUnits);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductUnit>> GetById(int id)
        {
            var productUnit = await _context.ProductUnits
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productUnit == null)
            {
                return NotFound();
            }

            return Ok(productUnit);
        }
    }
}