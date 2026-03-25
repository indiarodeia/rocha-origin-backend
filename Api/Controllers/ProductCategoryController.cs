using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/product-categories")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductCategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetAllActive()
        {
            var productCategories = await _context.ProductCategories
                .AsNoTracking()
                .Where(p => p.IsActive)
                .OrderBy(p => p.Order)
                .ToListAsync();

            return Ok(productCategories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductCategory>> GetById(int id)
        {
            var productCategory = await _context.ProductCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return Ok(productCategory);
        }
    }
}