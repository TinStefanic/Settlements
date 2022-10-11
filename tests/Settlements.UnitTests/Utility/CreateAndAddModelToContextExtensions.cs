using Settlements.Server.Data;
using Settlements.Server.Data.Models;

namespace Settlements.UnitTests.Utility
{
    public static class CreateAndAddModelToContextExtensions
    {
        public static async Task<Country> CreateAndAddCountryAsync(
            this SettlementsContext context,
            int id = 0,
            string name = "countryName",
            string regexPatern = "^.*$")
        {
            var country = new Country
            {
                Id = id,
                Name = name,
                RegexPattern = regexPatern
            };

            context.Countries.Add(country);
            await context.SaveChangesAsync();

            return country;
        }

        public static async Task<Settlement> CreateAndAddSettlementAsync(
            this SettlementsContext context,
            int id = 0,
            int countryId = 0,
            string name = "settlementName",
            string postalCode = "12345")
        {
            var settlement = new Settlement
            {
                Id = id,
                CountryId = countryId,
                Name = name,
                PostalCode = postalCode
            };

            context.Settlements.Add(settlement);
            await context.SaveChangesAsync();

            return settlement;
        }
    }
}
