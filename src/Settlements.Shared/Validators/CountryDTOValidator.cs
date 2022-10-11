using FluentValidation;
using Settlements.Shared.DTOs;

namespace Settlements.Shared.Validators
{
    public class CountryDTOValidator : AbstractValidator<CountryDTO>
    {
        public CountryDTOValidator()
        {
            RuleFor(c => c.Name)
                .NotNull()
                .Length(2, 128)
                .Matches("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$")
                .WithMessage("'{PropertyValue}' is not a valid country name."); ;
            RuleFor(c => c.RegexPattern).NotEmpty();
        }
    }
}
