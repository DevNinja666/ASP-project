using InvoiceManager.Api.Data;
using InvoiceManager.Api.DTOs;
using InvoiceManager.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManager.Api.Controllers;

/// <summary>
/// Управление клиентами
/// </summary>
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить список клиентов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
    {
        return await _context.Customers.ToListAsync();
    }

    /// <summary>
    /// Получить клиента по Id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> Get(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return NotFound();

        return customer;
    }

    /// <summary>
    /// Добавить клиента
    /// </summary>
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

    /// <summary>
    /// Редактировать клиента
    /// </summary>
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

        await _context.SaveChangesAsync();

        return Ok(customer);
    }

    /// <summary>
    /// Удаление клиента (hard delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers
            .Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null)
            return NotFound();

        if (customer.Invoices.Any(i => i.Status != InvoiceStatus.Created))
            return BadRequest("Нельзя удалить клиента с отправленными инвойсами");

        _context.Customers.Remove(customer);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Архивировать клиента (soft delete)
    /// </summary>
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
