using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Settlements.Server.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Settlements.Server.Services.ValidationService
{
	public interface ICustomSettlementValidationService
	{
		/// <summary>
		/// Verifies if provided postal code is valid postal code for the given country. 
		/// </summary>
		/// <param name="postalCode">Postal code to verify.</param>
		/// <param name="countryId">Id of intended country for provided postal code.</param>
		/// <returns>Is postal code valid, and if not, error message with details.</returns>
		ValidationResult? VerifyPostalCode(string postalCode, int countryId);

		/// <summary>
		/// Verifies if the provided country exists in database.
		/// </summary>
		/// <param name="countryId">Id of country to check.</param>
		/// <returns>Does country exist, and if it doesn't, error message with details. </returns>
		ValidationResult? VerifyCountryExists(int countryId);
	}
}
