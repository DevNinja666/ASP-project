using FluentValidation;
using Invoicer.DTOs.User;

namespace Invoicer.Validators.User
{
  
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
