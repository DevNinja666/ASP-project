using InvoiceManager.Api.Data;
using InvoiceManager.Api.DTOs;
using InvoiceManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManager.Api.Controllers;

[ApiController]
[Route("api/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public InvoicesController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetAll()
    {
        return await _context.Invoices
            .Include(i => i.Rows)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> Get(int id)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Rows)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        return invoice;
    }
    [HttpPost]
    public async Task<ActionResult<Invoice>> Create(CreateInvoiceDto dto)
    {
        var invoice = new Invoice
        {
            CustomerId = dto.CustomerId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Comment = dto.Comment,
            Status = InvoiceStatus.Created,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        foreach (var row in dto.Rows)
        {
            invoice.Rows.Add(new InvoiceRow
            {
                Service = row.Service,
                Quantity = row.Quantity,
                Rate = row.Rate
            });
        }

        invoice.TotalSum = invoice.Rows.Sum(r => r.Sum);

        _context.Invoices.Add(invoice);

        await _context.SaveChangesAsync();

        return Ok(invoice);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateInvoiceDto dto)
    {
        var invoice = await _context.Invoices
            .Include(i => i.Rows)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            return NotFound();

        if (invoice.Status != InvoiceStatus.Created)
            return BadRequest("Редактировать можно только не отправленные инвойсы");

        invoice.StartDate = dto.StartDate;
        invoice.EndDate = dto.EndDate;
        invoice.Comment = dto.Comment;

        _context.InvoiceRows.RemoveRange(invoice.Rows);

        invoice.Rows = dto.Rows.Select(r => new InvoiceRow
        {
            Service = r.Service,
            Quantity = r.Quantity,
            Rate = r.Rate
        }).ToList();

        invoice.TotalSum = invoice.Rows.Sum(r => r.Quantity * r.Rate);

        await _context.SaveChangesAsync();

        return Ok(invoice);
    }

 
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, InvoiceStatus status)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
            return NotFound();

        invoice.Status = status;

        await _context.SaveChangesAsync();

        return Ok(invoice);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
            return NotFound();

        if (invoice.Status != InvoiceStatus.Created)
            return BadRequest("Удалять можно только не отправленные инвойсы");

        _context.Invoices.Remove(invoice);

        await _context.SaveChangesAsync();

        return NoContent();
    }

\
    [HttpPatch("{id}/archive")]
    public async Task<IActionResult> Archive(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice == null)
            return NotFound();

        invoice.DeletedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
