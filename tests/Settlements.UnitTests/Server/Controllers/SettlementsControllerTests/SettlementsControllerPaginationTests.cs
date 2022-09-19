using Settlements.UnitTests.Utility;

namespace Settlements.UnitTests.Server.Controllers.SettlementsControllerTests
{
	public class SettlementsControllerPaginationTests
	{
		[Fact]
		public async Task GetPaginatedSettlements_ShouldReturnCorrectTotalSettlementCount()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = 
				await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb", postalCode: "10000");
			var split = 
				await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Split", postalCode: "21000");
			var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.GetPaginatedSettlements();

			result.Value?.TotalSettlementsCount.Should().Be(2);
		}

		[Fact]
		public async Task GetPaginatedSettlements_ShouldReturnCorrectAmountOfSettlements_WhenPageSizeIsntSpecified()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb =
				await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb", postalCode: "10000");
			var split =
				await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Split", postalCode: "21000");
			var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.GetPaginatedSettlements();

			result.Value?.Settlements.Should().HaveCount(2);
		}

		[Fact]
		public async Task GetPaginatedSettlements_ShouldReturnCorrectAmountOfSettlements_WhenPageSizeIs1()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb =
				await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb", postalCode: "10000");
			var split =
				await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Split", postalCode: "21000");
			var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.GetPaginatedSettlements(pageSize: 1);

			result.Value?.Settlements.Should().HaveCount(1);
		}

		[Fact]
		public async Task GetPaginatedSettlements_ShouldReturn2ndSettlementInDb()
		{
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb =
				await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb", postalCode: "10000");
			var split =
				await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Split", postalCode: "21000");
			var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.GetPaginatedSettlements(pageNumber: 2, pageSize: 1);

			result.Value?.Settlements?.First()?.Id.Should().Be(split.Id);
		}
	}
}
