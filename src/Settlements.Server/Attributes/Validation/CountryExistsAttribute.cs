using Settlements.Server.Services.ValidationService;
using System.ComponentModel.DataAnnotations;

namespace Settlements.Server.Attributes.Validation
{
	public class CountryExistsAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			int countryId = ((int?)value) ?? -1;

			var settlementValidationService = 
				(CustomSettlementValidationService)validationContext
				.GetService(typeof(ICustomSettlementValidationService))!;

			return settlementValidationService.VerifyCountryExists(countryId);
		}
	}
}
