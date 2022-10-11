using FluentValidation.TestHelper;
using Settlements.Server.Validators;
using Settlements.Shared.DTOs;
using Settlements.UnitTests.Utility;

namespace Settlements.UnitTests.Server.Validators
{
    public class SettlementDTOServerValidatorTests
    {
        [Fact]
        public async Task ShouldFailForCountryIdThatDoesntExist()
        {
            using var settlementContextFactory = new SettlementsContextFactory();
            using var context = settlementContextFactory.CreateContext();
            var validator = new SettlementDTOServerValidator(context);
            var settlement = new SettlementDTO
            {
                Id = 1,
                CountryId = 3,
                Name = "Zagreb",
                PostalCode = "10000"
            };

            var result = await validator.TestValidateAsync(settlement);

            result
                .ShouldHaveValidationErrorFor(s => s.CountryId)
                .WithErrorMessage($"Country with id '{settlement.CountryId}' doesn't exist in database.");
        }

        [Fact]
        public async Task ShouldFailForPostalCodeThatIsntValidForCountry()
        {
            using var settlementContextFactory = new SettlementsContextFactory();
            using var context = settlementContextFactory.CreateContext();
            var croatia = await context.CreateAndAddCountryAsync(name: "Croatia", regexPatern: "^\\d{5}$");
            var validator = new SettlementDTOServerValidator(context);
            var settlement = new SettlementDTO
            {
                Id = 1,
                CountryId = croatia.Id,
                Name = "Zagreb",
                PostalCode = "1234567"
            };

            var result = await validator.TestValidateAsync(settlement);

            result
                .ShouldHaveValidationErrorFor(s => s.PostalCode)
                .WithErrorMessage(
                    $"Postal code '{settlement.PostalCode}' isn't valid postal code of country '{croatia.Name}'."
                );
        }

        [Fact]
        public async Task ShouldFailForPostalCodeCountryCombinationThatIsAlreadyPresentInDatabase()
        {
            using var settlementContextFactory = new SettlementsContextFactory();
            using var context = settlementContextFactory.CreateContext();
            var croatia = await context.CreateAndAddCountryAsync(name: "Croatia", regexPatern: "^\\d{5}$");
            var zagreb =
                await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb", postalCode: "10000");
            var validator = new SettlementDTOServerValidator(context);
            var settlement = new SettlementDTO
            {
                Id = zagreb.Id + 1,
                CountryId = zagreb.CountryId,
                Name = "Some City",
                PostalCode = zagreb.PostalCode
            };

            var result = await validator.TestValidateAsync(settlement);

            result
                .ShouldHaveValidationErrorFor(s => s.PostalCode)
                .WithErrorMessage(
                    $"Settlement '{zagreb.Name}' already has provided postal code '{settlement.PostalCode}'"
                    + " in the same country."
                );
        }

        [Fact]
        public async Task ShouldSucceed()
        {
            using var settlementContextFactory = new SettlementsContextFactory();
            using var context = settlementContextFactory.CreateContext();
            var croatia = await context.CreateAndAddCountryAsync(name: "Croatia", regexPatern: "^\\d{5}$");
            var validator = new SettlementDTOServerValidator(context);
            var settlement = new SettlementDTO
            {
                Id = 1,
                CountryId = croatia.Id,
                Name = "Zagreb",
                PostalCode = "10000"
            };

            var result = await validator.TestValidateAsync(settlement);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
