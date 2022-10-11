using Settlements.UnitTests.Utility;

namespace Settlements.UnitTests.Server.Controllers
{
    public class CountriesControllerTests
    {
        [Fact]
        public async Task GetCountries_ShouldReturnCorrectNumberOfCountries()
        {
            using var settlementContextFactory = new SettlementsContextFactory();
            using var context = settlementContextFactory.CreateContext();
            var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
            var japan = await context.CreateAndAddCountryAsync(name: "Japan");
            var controller = ControllerFactory.CreateCountriesController(context);

            var result = await controller.GetCountries();

            result?.Value.Should().HaveCount(2);
        }
    }
}
