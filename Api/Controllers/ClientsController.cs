using Api.Data;
using Api.Dtos;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> Get()
        {
            var clients = await _context.Clients
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Include(x => x.BillingAddress)
                .Include(x => x.PaymentType)
                .ToListAsync();

            return Ok(clients);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Client>> GetById(Guid id)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .Include(x => x.BillingAddress)
                .Include(x => x.PaymentType)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (client is null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPost("search-and-filter")]
        public async Task<ActionResult<IEnumerable<Client>>> SearchAndFilterClients([FromBody] SearchAndFilterClientsRequestDto request, CancellationToken cancellationToken)
        {
            var search = request.Search?.Trim();
            var city = request.City?.Trim();

            IQueryable<Client> query = _context.Clients
                .AsNoTracking()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => EF.Functions.ILike(x.CompanyName, $"%{search}%"));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(x =>
                    x.BillingAddress != null &&
                    EF.Functions.ILike(x.BillingAddress.City, $"%{city}%"));
            }

            if (request.PaymentTypeId.HasValue)
            {
                query = query.Where(x => x.PaymentTypeId == request.PaymentTypeId.Value);
            }

            query = request.SortBy?.Trim().ToLowerInvariant() switch
            {
                "createdat" => request.IsSortAscending
                    ? query.OrderBy(x => x.CreatedAt).ThenBy(x => x.CompanyName)
                    : query.OrderByDescending(x => x.CreatedAt).ThenBy(x => x.CompanyName),

                _ => request.IsSortAscending
                    ? query.OrderBy(x => x.CompanyName).ThenByDescending(x => x.CreatedAt)
                    : query.OrderByDescending(x => x.CompanyName).ThenByDescending(x => x.CreatedAt)
            };

            var clients = await query
                .Include(x => x.BillingAddress)
                .Include(x => x.PaymentType)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return Ok(clients);
        }

        [HttpPost("search-and-filter/count")]
        public async Task<ActionResult<int>> SearchAndFilterClientsCount([FromBody] SearchAndFilterClientsRequestDto request, CancellationToken cancellationToken)
        {
            var search = request.Search?.Trim();
            var city = request.City?.Trim();

            IQueryable<Client> query = _context.Clients
                .AsNoTracking()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => EF.Functions.ILike(x.CompanyName, $"%{search}%"));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(x =>
                    x.BillingAddress != null &&
                    EF.Functions.ILike(x.BillingAddress.City, $"%{city}%"));
            }

            if (request.PaymentTypeId.HasValue)
            {
                query = query.Where(x => x.PaymentTypeId == request.PaymentTypeId.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            return Ok(totalCount);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Create([FromBody] Client model)
        {
            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Clients.Add(model);
            await _context.SaveChangesAsync();

            var createdClient = await _context.Clients
                .AsNoTracking()
                .Include(x => x.BillingAddress)
                .Include(x => x.PaymentType)
                .FirstAsync(x => x.Id == model.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdClient.Id }, createdClient);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Client>> Update(Guid id, [FromBody] Client model)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (client is null)
            {
                return NotFound();
            }

            client.CompanyName = model.CompanyName;
            client.VatNumber = model.VatNumber;
            client.Phone = model.Phone;
            client.Email = model.Email;
            client.IsActive = model.IsActive;
            client.BillingAddressId = model.BillingAddressId;
            client.Notes = model.Notes;
            client.PaymentTypeId = model.PaymentTypeId;
            client.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedClient = await _context.Clients
                .AsNoTracking()
                .Include(x => x.BillingAddress)
                .Include(x => x.PaymentType)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedClient);
        }

        [HttpPatch("{id:guid}/set-as-inactive")]
        public async Task<ActionResult<Client>> SetAsInactive(Guid id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (client is null)
            {
                return NotFound();
            }

            client.IsActive = false;
            client.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var updatedClient = await _context.Clients
                .AsNoTracking()
                .Include(x => x.BillingAddress)
                .Include(x => x.PaymentType)
                .FirstAsync(x => x.Id == id);

            return Ok(updatedClient);
        }
    }
}