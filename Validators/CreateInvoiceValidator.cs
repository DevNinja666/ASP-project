using FluentValidation;
using InvoiceManager.Api.DTOs;

namespace InvoiceManager.Api.Validators;

public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceDto>
{
    public CreateInvoiceValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate);

        RuleFor(x => x.Rows)
            .NotEmpty();
    }
}
