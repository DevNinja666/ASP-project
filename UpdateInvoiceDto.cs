namespace InvoiceManager.Api.DTOs;

public class UpdateInvoiceDto
{
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string? Comment { get; set; }
    public List<InvoiceRowDto> Rows { get; set; } = new();
}
