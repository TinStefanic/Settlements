using Settlements.Server.Services.ValidationService;
using System.ComponentModel.DataAnnotations;

namespace Settlements.Server.Attributes.Validation
{
	public class PostalCodeBelongsToCountryAttribute : ValidationAttribute
	{
		readonly string _countryNameProperty;

		public PostalCodeBelongsToCountryAttribute(string countryNameProperty)
		{
			_countryNameProperty = countryNameProperty;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			string postalCode = value?.ToString() ?? "";

			var property = validationContext.ObjectType.GetProperty(_countryNameProperty);

			if (property is null)
			{
				throw new ArgumentException(
					$"Property with name '{_countryNameProperty}' not found "
					+ $"on object of type '{validationContext.ObjectType}'."
				);
			}

			int countryId = ((int?)property.GetValue(validationContext.ObjectInstance)) ?? -1;

			var settlementValidationService =
				(CustomSettlementValidationService)validationContext
				.GetService(typeof(ICustomSettlementValidationService))!;

			return settlementValidationService.VerifyPostalCode(postalCode, countryId);
		}
	}
}
