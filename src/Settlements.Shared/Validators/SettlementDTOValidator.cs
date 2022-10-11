using FluentValidation;
using Settlements.Shared.DTOs;

namespace Settlements.Shared.Validators
{
    public class SettlementDTOValidator : AbstractValidator<SettlementDTO>
    {
        public SettlementDTOValidator()
        {
            RuleFor(s => s.CountryId).NotNull();
            RuleFor(s => s.Name)
                .NotNull()
                .Length(2, 128)
                .Matches("^[a-zA-Z]+(?:[\\s-][a-zA-Z]+)*$")
                .WithMessage("'{PropertyValue}' is not a valid settlement name.");
            RuleFor(s => s.PostalCode).NotNull().Length(4, 16);
        }
    }
}
