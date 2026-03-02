namespace InvoiceManager.Api.DTOs;

public class CreateCustomerDto
{
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}
