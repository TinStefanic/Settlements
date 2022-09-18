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
					$"Postal code '{postalCode}' doesn't match postal codes of country '{country.Name}'."
				);
			}
		}
	}
}
