using Settlements.Server.Data.Models;
using Settlements.Server.Services.ValidationService;
using Settlements.UnitTests.Utility;
using System.ComponentModel.DataAnnotations;

namespace Settlements.UnitTests.Server.Services.ValidationService
{
	public class CustomSettlementValidationServiceTests
	{
		[Fact]
		public async Task VerifyCountryExists_ShouldReturnValidationSuccessfull()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var settlementValidator = new CustomSettlementValidationService(context);

			var result = settlementValidator.VerifyCountryExists(croatia.Id);

			result.Should().Be(ValidationResult.Success);
		}

		[Fact]
		public void VerifyCountryExist_ShouldReturnValidationFailed()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var settlementValidator = new CustomSettlementValidationService(context);

			var result = settlementValidator.VerifyCountryExists(3);

			result.Should().NotBe(ValidationResult.Success);
		}

		[Fact]
		public async Task VerifyPostalCode_ShouldReturnValidationSuccessfull()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia", regexPatern: "^\\d{5}$");
			var settlementValidator = new CustomSettlementValidationService(context);

			var result = settlementValidator.VerifyPostalCode(postalCode: "10000", croatia.Id);

			result.Should().Be(ValidationResult.Success);
		}

		[Fact]
		public async Task VerifyPostalCode_ShouldReturnValidationFailed()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia", regexPatern: "^\\d{5}$");
			var settlementValidator = new CustomSettlementValidationService(context);

			var result = settlementValidator.VerifyPostalCode(postalCode: "1234567", croatia.Id);

			result.Should().NotBe(ValidationResult.Success);
		}

		[Fact]
		public async Task VerifyPostalCodeIsntAlreadyPresentForTheCountry_ShouldReturnValidationSuccessfull()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia", regexPatern: "^\\d{5}$");
			var settlementValidator = new CustomSettlementValidationService(context);

			var result = 
				settlementValidator.VerifyPostalCodeIsntAlreadyPresentForTheCountry(postalCode: "10000", croatia.Id);

			result.Should().Be(ValidationResult.Success);
		}

		[Fact]
		public async Task VerifyPostalCodeIsntAlreadyPresentForTheCountry_ShouldReturnValidationFailed()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia", regexPatern: "^\\d{5}$");
			var zabreb = 
				await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb", postalCode: "10000");
			var settlementValidator = new CustomSettlementValidationService(context);

			var result = 
				settlementValidator.VerifyPostalCodeIsntAlreadyPresentForTheCountry(postalCode: "10000", croatia.Id);

			result.Should().NotBe(ValidationResult.Success);
		}
	}
}
