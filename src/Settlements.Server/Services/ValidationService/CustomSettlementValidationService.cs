using Settlements.Server.Data;
using Settlements.Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Settlements.Server.Services.ValidationService
{
	public class CustomSettlementValidationService : ICustomSettlementValidationService
	{
		private readonly SettlementsContext _context;

		public CustomSettlementValidationService(SettlementsContext context)
		{
			_context = context;
		}

		public ValidationResult? VerifyCountryExists(int countryId)
		{
			Country? country = _context.Countries.Find(countryId);

			if (country is null)
			{
				return new ValidationResult($"Country with id '{countryId}' doesn't exist in database.");
			}
			else
			{
				return ValidationResult.Success;
			}
		}

		public ValidationResult? VerifyPostalCode(string postalCode, int countryId)
		{
			Country? country = _context.Countries.Find(countryId);

			if (country is null)
			{
				return ValidationResult.Success; // Nothing to verify postal code with.
			}

			if (Regex.IsMatch(postalCode, country.RegexPattern))
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult(
					$"Postal code '{postalCode}' isn't valid postal code of country '{country.Name}'."
				);
			}
		}

		public ValidationResult? VerifyPostalCodeIsntAlreadyPresentForTheCountry(string postalCode, int countryId)
		{
			var settlement = _context.Settlements.FirstOrDefault(
				s => s.PostalCode == postalCode && s.CountryId == countryId
			);

			if (settlement is null)
			{
				return ValidationResult.Success;
			}

			else
			{
				return new ValidationResult(
					$"Settlement '{settlement.Name}' already has provided postal code '{postalCode}'"
					+ " in the same country."
				);
			}
		}
	}
}
