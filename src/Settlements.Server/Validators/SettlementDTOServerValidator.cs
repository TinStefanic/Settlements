using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Settlements.Server.Data;
using Settlements.Server.Data.Models;
using Settlements.Shared.DTOs;
using Settlements.Shared.Validators;

namespace Settlements.Server.Validators
{
    public class SettlementDTOServerValidator : AbstractValidator<SettlementDTO>
    {
        private readonly SettlementsContext _context;

        public SettlementDTOServerValidator(SettlementsContext context)
        {
            _context = context;

            Include(new SettlementDTOValidator());

            RuleFor(s => s.CountryId)
                .MustAsync(VerifyCountryExistsAsync)
                .WithMessage("Country with id '{PropertyValue}' doesn't exist in database.");

            RuleFor(s => s.PostalCode)
                .MustAsync(VerifyPostalCodeAsync)
                .WithMessage("Postal code '{PropertyValue}' isn't valid postal code of country '{CountryName}'.");

            RuleFor(s => s.PostalCode)
                .MustAsync(VerifyPostalCodeIsntAlreadyPresentForTheCountryAsync)
                .WithMessage(
                    "Settlement '{ConflictingSettlementName}' already has provided postal code '{PropertyValue}'"
                    + " in the same country."
                );
        }

        private async Task<bool> VerifyCountryExistsAsync(int? countryId, CancellationToken cancellation)
        {
            Country? country =
                await _context.Countries.FindAsync(new object?[] { countryId }, cancellationToken: cancellation);

            return country is not null;
        }

        private async Task<bool> VerifyPostalCodeAsync(
            SettlementDTO settlement,
            string? postalCode,
            ValidationContext<SettlementDTO> context,
            CancellationToken cancellation)
        {
            Country? country =
                await _context.Countries.FindAsync(
                    new object?[] { settlement.CountryId },
                    cancellationToken: cancellation
                );

            if (country is null || settlement.PostalCode is null)
            {
                return true; // Nothing to verify postal code with.
            }

            if (Regex.IsMatch(settlement.PostalCode, country.RegexPattern))
            {
                return true;
            }
            else
            {
                context.MessageFormatter.AppendArgument("CountryName", country.Name);
                return false;
            }
        }

        private async Task<bool> VerifyPostalCodeIsntAlreadyPresentForTheCountryAsync(
            SettlementDTO settlement,
            string? postalCode,
            ValidationContext<SettlementDTO> context,
            CancellationToken cancellation)
        {
            var conflictingSettlement = await _context.Settlements.FirstOrDefaultAsync(
                s => s.PostalCode == settlement.PostalCode && s.CountryId == settlement.CountryId,
                cancellationToken: cancellation
            );

            if (conflictingSettlement is null || conflictingSettlement.Id == settlement.Id)
            {
                return true; // Settlement has nothing to conflict with, or conflicting settlement is itself.
            }

            else
            {
                context.MessageFormatter.AppendArgument("ConflictingSettlementName", conflictingSettlement.Name);
                return false;
            }
        }
    }
}
