using InvoiceManager.Api.Data;
using InvoiceManager.Api.DTOs;
using InvoiceManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManager.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

[HttpGet]
public async Task<IActionResult> GetAll(
    int userId,
    int page = 1,
    int pageSize = 10,
    string? search = null,
    string? sortBy = null)
{
    var query = _context.Customers
        .Where(c => c.UserId == userId && c.DeletedAt == null)
        .AsQueryable();

    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(c => c.Name.Contains(search));
    }

    if (sortBy == "name")
        query = query.OrderBy(c => c.Name);

    if (sortBy == "email")
        query = query.OrderBy(c => c.Email);

    var total = await query.CountAsync();

    var customers = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Ok(new
    {
        total,
        page,
        pageSize,
        data = customers
    });[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    var customer = await _context.Customers.FindAsync(id);

    if (customer == null)
        return NotFound();

    var hasSentInvoices = await _context.Invoices
        .AnyAsync(i => i.CustomerId == id && i.Status != InvoiceStatus.Created);

    if (hasSentInvoices)
        return BadRequest("Cannot delete customer with sent invoices");

    _context.Customers.Remove(customer);
    await _context.SaveChangesAsync();

    return Ok();
    [HttpPatch("{id}/archive")]
public async Task<IActionResult> Archive(int id)
{
    var customer = await _context.Customers.FindAsync(id);

    if (customer == null)
        return NotFound();

    customer.DeletedAt = DateTimeOffset.UtcNow;
    customer.UpdatedAt = DateTimeOffset.UtcNow;

    await _context.SaveChangesAsync();

    return Ok();
}
}
}
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationQuery pagination,
        string? search,
        string? sortBy)
    {
        var query = _context.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            switch (sortBy.ToLower())
            {
                case "name":
                    query = query.OrderBy(c => c.Name);
                    break;

                case "email":
                    query = query.OrderBy(c => c.Email);
                    break;

                default:
                    query = query.OrderBy(c => c.Id);
                    break;
            }
        }
        else
        {
            query = query.OrderBy(c => c.Id);
        }

        var total = await query.CountAsync();

    
        var customers = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return Ok(new
        {
            totalItems = total,
            page = pagination.Page,
            pageSize = pagination.PageSize,
            items = customers
        });
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> Get(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return NotFound();

        return customer;
    }

   
    [HttpPost]
    public async Task<ActionResult<Customer>> Create(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Address = dto.Address,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Customers.Add(customer);

        await _context.SaveChangesAsync();

        return Ok(customer);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return NotFound();

        customer.Name = dto.Name;
        customer.Address = dto.Address;
        customer.Email = dto.Email;
        customer.PhoneNumber = dto.PhoneNumber;

        customer.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(customer);
    }

\
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers
            .Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null)
            return NotFound();

        if (customer.Invoices.Any(i => i.Status != InvoiceStatus.Created))
            return BadRequest("Нельзя удалить клиента, у которого есть отправленные инвойсы");

        _context.Customers.Remove(customer);

        await _context.SaveChangesAsync();

        return NoContent();
    }

 
    [HttpPatch("{id}/archive")]
    public async Task<IActionResult> Archive(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return NotFound();

        customer.DeletedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
