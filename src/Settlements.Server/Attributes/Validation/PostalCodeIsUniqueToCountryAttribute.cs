using Settlements.Server.Services.ValidationService;
using System.ComponentModel.DataAnnotations;

namespace Settlements.Server.Attributes.Validation
{
    public class PostalCodeIsUniqueToCountryAttribute : ValidationAttribute
    {
		private readonly string _countryIdProperty;
		private readonly string _settlementIdProperty;

		public PostalCodeIsUniqueToCountryAttribute(string countryIdProperty, string settlementIdProperty)
		{
			_countryIdProperty = countryIdProperty;
			_settlementIdProperty = settlementIdProperty;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			string postalCode = value?.ToString() ?? "";

			var propertyCountryId = validationContext.ObjectType.GetProperty(_countryIdProperty);

			if (propertyCountryId is null)
			{
				throw new ArgumentException(
					$"Property with name '{_countryIdProperty}' not found "
					+ $"on object of type '{validationContext.ObjectType}'."
				);
			}

			var propertySettlementId = validationContext.ObjectType.GetProperty(_settlementIdProperty);

			if (propertySettlementId is null)
			{
				throw new ArgumentException(
					$"Property with name '{_settlementIdProperty}' not found "
					+ $"on object of type '{validationContext.ObjectType}'."
				);
			}

			int countryId = ((int?)propertyCountryId.GetValue(validationContext.ObjectInstance)) ?? -1;
			int settlementId = ((int?)propertySettlementId.GetValue(validationContext.ObjectInstance)) ?? -1;

			var settlementValidationService =
				(CustomSettlementValidationService)validationContext
				.GetService(typeof(ICustomSettlementValidationService))!;

			return settlementValidationService
				.VerifyPostalCodeIsntAlreadyPresentForTheCountry(postalCode, countryId, settlementId);
		}
	}
}
