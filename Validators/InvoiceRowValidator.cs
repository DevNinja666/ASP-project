using FluentValidation;
using InvoiceManager.Api.DTOs;

namespace InvoiceManager.Api.Validators;

public class InvoiceRowValidator : AbstractValidator<InvoiceRowDto>
{
    public InvoiceRowValidator()
    {
        RuleFor(x => x.Service)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);

        RuleFor(x => x.Rate)
            .GreaterThan(0);
    }
}
