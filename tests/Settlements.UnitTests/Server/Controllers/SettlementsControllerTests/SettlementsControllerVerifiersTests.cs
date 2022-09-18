using Settlements.Server.Controllers;
using Settlements.Server.Data.Models;
using Settlements.Server.Services.ValidationService;
using Settlements.UnitTests.Utility;

namespace Settlements.UnitTests.Server.Controllers.SettlementsControllerTests
{
	public class SettlementsControllerVerifiersTests
	{
		[Fact]
		public void VerifyCountry_ShouldReturnErrorMessage_WhenCountryDoesntExist()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var controller = new SettlementsController(context, new CustomSettlementValidationService(context));

			var result = controller.VerifyCountry(1);

			result.Value.Should().BeOfType<string>();
		}

		[Fact]
		public async Task VerifyCountry_ShouldReturnTrue_WhenCountryExists()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = new Country { Name = "Croatia", RegexPattern = "^\\d{5}$" };
			context.Countries.Add(croatia);
			await context.SaveChangesAsync();
			var controller = new SettlementsController(context, new CustomSettlementValidationService(context));

			var result = controller.VerifyCountry(croatia.Id);

			result.Value.Should().Be(true);
		}

		[Fact]
		public async Task VerifyPostalCode_ShouldReturnErrorMessage_WhenPostalCodeDoesntMatchCountryRegex()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = new Country { Name = "Croatia", RegexPattern = "^\\d{5}$" };
			context.Countries.Add(croatia);
			await context.SaveChangesAsync();
			var controller = new SettlementsController(context, new CustomSettlementValidationService(context));
			string postalCode = "1234567";

			var result = controller.VerifyPostalCode(postalCode, croatia.Id);

			result.Value.Should().BeOfType<string>();
		}

		[Fact]
		public async Task VerifyPostalCode_ShouldReturnTrue_WhenPostalCodeMathesCountryRegex()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = new Country { Name = "Croatia", RegexPattern = "^\\d{5}$" };
			context.Countries.Add(croatia);
			await context.SaveChangesAsync();
			var controller = new SettlementsController(context, new CustomSettlementValidationService(context));
			string postalCode = "10000";

			var result = controller.VerifyPostalCode(postalCode, croatia.Id);

			result.Value.Should().Be(true);
		}
	}
}
