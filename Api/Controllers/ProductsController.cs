using Api.Data;
using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Include(x => x.Category)
                .Include(x => x.DefaultUnit)
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Product>> GetById(Guid id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.DefaultUnit)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost("search-and-filter")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchAndFilterProducts([FromBody] SearchAndFilterProductsRequestDto request, CancellationToken cancellationToken)
        {
            var search = request.Search?.Trim();

            IQueryable<Product> query = _context.Products
                .AsNoTracking();

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }
            else
            {
                query = query.Where(x => x.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    EF.Functions.ILike(x.Name, $"%{search}%") ||
                    (x.Category != null && EF.Functions.ILike(x.Category.Name, $"%{search}%")));
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(x => x.ProductCategoryId == request.CategoryId.Value);
            }

            if (request.VatRate.HasValue)
            {
                query = query.Where(x => x.DefaultVatRate.HasValue && x.DefaultVatRate.Value == request.VatRate.Value);
            }

            query = request.SortBy?.Trim().ToLowerInvariant() switch
            {
                "category" => request.IsSortAscending
                    ? query.OrderBy(x => x.Category != null ? x.Category.Name : string.Empty)
                        .ThenBy(x => x.Name)
                    : query.OrderByDescending(x => x.Category != null ? x.Category.Name : string.Empty)
                        .ThenBy(x => x.Name),

                "price" => request.IsSortAscending
                    ? query.OrderBy(x => x.DefaultSellPrice ?? decimal.MaxValue)
                        .ThenBy(x => x.Name)
                    : query.OrderByDescending(x => x.DefaultSellPrice ?? decimal.MinValue)
                        .ThenBy(x => x.Name),

                "status" => request.IsSortAscending
                    ? query.OrderBy(x => x.IsActive)
                        .ThenBy(x => x.Name)
                    : query.OrderByDescending(x => x.IsActive)
                        .ThenBy(x => x.Name),

                _ => request.IsSortAscending
                    ? query.OrderBy(x => x.Name)
                        .ThenBy(x => x.ProductCategoryId)
                    : query.OrderByDescending(x => x.Name)
                        .ThenBy(x => x.ProductCategoryId)
            };

            var products = await query
                .Include(x => x.Category)
                .Include(x => x.DefaultUnit)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return Ok(products);
        }

        [HttpPost("search-and-filter/count")]
        public async Task<ActionResult<int>> SearchAndFilterProductsCount([FromBody] SearchAndFilterProductsRequestDto request, CancellationToken cancellationToken)
        {
            var search = request.Search?.Trim();

            IQueryable<Product> query = _context.Products
                .AsNoTracking();

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }
            else
            {
                query = query.Where(x => x.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    EF.Functions.ILike(x.Name, $"%{search}%") ||
                    (x.Category != null && EF.Functions.ILike(x.Category.Name, $"%{search}%")));
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(x => x.ProductCategoryId == request.CategoryId.Value);
            }

            if (request.VatRate.HasValue)
            {
                query = query.Where(x => x.DefaultVatRate.HasValue && x.DefaultVatRate.Value == request.VatRate.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            return Ok(totalCount);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product model)
        {
            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            var createdProduct = await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.DefaultUnit)
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Product>> Update(Guid id, [FromBody] Product model)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.ProductCategoryId = model.ProductCategoryId;
            product.DefaultUnitId = model.DefaultUnitId;
            product.DefaultVatRate = model.DefaultVatRate;
            product.DefaultSellPrice = model.DefaultSellPrice;
            product.InternalCode = model.InternalCode;
            product.Description = model.Description;
            product.IsActive = model.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedProduct = await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.DefaultUnit)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedProduct);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<Product>> SetAsInactive(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedProduct = await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.DefaultUnit)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedProduct);
        }
    }
}