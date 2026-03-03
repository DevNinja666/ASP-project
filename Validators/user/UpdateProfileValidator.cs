using FluentValidation;
using Invoicer.DTOs.User;

namespace Invoicer.Validators.User
{
 
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20)
                .When(x => x.PhoneNumber != null);

            RuleFor(x => x.Address)
                .MaximumLength(250)
                .When(x => x.Address != null);
        }
    }
}
