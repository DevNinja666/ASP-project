using FluentValidation;
using Invoicer.DTOs.User;

namespace Invoicer.Validators.User
{
//nadir muellim eto dla smeni parola sdelal
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
