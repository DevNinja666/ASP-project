using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaxi.Data;
using SmartTaxi.Models;

namespace SmartTaxi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(
            int page = 1,
            int pageSize = 10,
            string? search = null,
            string? sortBy = null)
        {
            var query = _context.Invoices
                .Include(i => i.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(i => i.Customer.Name.Contains(search));
            }
[HttpGet]
public async Task<IActionResult> GetAll(
    int page = 1,
    int pageSize = 10,
    string? status = null,
    string? sortBy = null)
{
    var query = _context.Invoices
        .Include(i => i.Rows)
        .Where(i => i.DeletedAt == null)
        .AsQueryable();

    if (!string.IsNullOrEmpty(status))
    {
        query = query.Where(i => i.Status.ToString() == status);
    }

    if (sortBy == "date")
        query = query.OrderBy(i => i.StarDate);

    if (sortBy == "sum")
        query = query.OrderByDescending(i => i.TotalSum);

    var total = await query.CountAsync();

    var invoices = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Ok(new
    {
        total,
        page,
        pageSize,
        data = invoices
    });
}
            if (sortBy == "amount")
                query = query.OrderBy(i => i.Amount);

            if (sortBy == "date")
                query = query.OrderBy(i => i.Date);

            var total = await query.CountAsync();

            var invoices = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                total,
                page,
                pageSize,
                data = invoices
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> Get(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            return invoice;
        }
        [HttpPost]
        public async Task<ActionResult<Invoice>> Create(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = invoice.Id }, invoice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Invoice invoice)
        {
            if (id != invoice.Id)
                return BadRequest();

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Invoices.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice == null)
                return NotFound();

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
