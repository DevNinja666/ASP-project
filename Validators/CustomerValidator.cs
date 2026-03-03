using FluentValidation;
using Invoicer.DTOs;

namespace Invoicer.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerDto>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20)
                .When(x => x.PhoneNumber != null);

            RuleFor(x => x.Address)
                .MaximumLength(300)
                .When(x => x.Address != null);
        }
    }
}
