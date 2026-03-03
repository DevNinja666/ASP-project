using FluentValidation;
using Invoicer.DTOs;

namespace Invoicer.Validators
{
    public class PaginationValidator : AbstractValidator<PaginationQuery>
    {
        public PaginationValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);
        }
    }
}
