using FluentValidation;
using Invoicer.DTOs;

namespace Invoicer.Validators
{
    public class InvoiceValidator : AbstractValidator<InvoiceDto>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);

            RuleFor(x => x.StarDate)
                .NotEmpty();

            RuleFor(x => x.EndDate)
                .NotEmpty();

            RuleFor(x => x.Rows)
                .NotEmpty();
        }
    }
}
