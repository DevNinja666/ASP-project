namespace InvoiceManager.Api.DTOs;

public class InvoiceRowDto
{
    public string Service { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal Rate { get; set; }
}
