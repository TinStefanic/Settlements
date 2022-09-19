using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Settlements.Server.Data.Models;
using Settlements.UnitTests.Utility;

namespace Settlements.UnitTests.Server.Controllers.SettlementsControllerTests
{
    public class SettlementsControllerCRUDTests
    {
        [Fact]
        public async Task GetSettlement_ShouldReturnCorrectSettlement()
        {
            using var settlementContextFactory = new SettlementsContextFactory();
            using var context = settlementContextFactory.CreateContext();
            var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
            var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
            var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.GetSettlement(zagreb.Id);

            result.Result.Should().BeOfType<OkObjectResult>();
            result.Value?.Name.Should().Be(zagreb.Name);
        }

        [Fact]
        public async Task GetSettlement_ShouldReturnNotFound_WhenSettlementDoesntExist()
        {
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
			var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.GetSettlement(zagreb.Id + 1);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateSettlement_ShouldReturnBadRequest_WhenIdsArentEqual()
        {
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
			var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.UpdateSettlement(zagreb.Id + 1, zagreb);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task UpdateSettlement_ShouldReturnNotFound_WhenSettlementDoesntExist()
        {
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
			var controller = ControllerFactory.CreateSettlementController(context);
			var splitUntracked =
                new Settlement { Id = zagreb.Id + 1, CountryId = croatia.Id, Name = "Split", PostalCode = "21000" };

            var result = await controller.UpdateSettlement(splitUntracked.Id, splitUntracked);

            result.Should().BeOfType<NotFoundResult>();
        }

		[Fact]
        public async Task UpdateSettlement_ShouldReturnNoContent_WhenSettlementWasSuccessfullyFound()
        {
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
			var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.UpdateSettlement(zagreb.Id, zagreb);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateSettlement_ShouldChangeNameOfSettlement()
        {
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
			var controller = ControllerFactory.CreateSettlementController(context);
			var split =
                new Settlement { Id = zagreb.Id, CountryId = croatia.Id, Name = "Split", PostalCode = "21000" };

            await controller.UpdateSettlement(zagreb.Id, split);
            var updatedSettlement = await context.Settlements.FirstOrDefaultAsync(s => s.Id == zagreb.Id);

            updatedSettlement?.Name.Should().Be(split.Name);
        }

        [Fact]
        public async Task CreateSettlement_ShouldReturnCreatedAtAction()
        {
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
			var controller = ControllerFactory.CreateSettlementController(context);
			var split = new Settlement { CountryId = croatia.Id, Name = "Split", PostalCode = "21000" };

            var result = await controller.CreateSettlement(split);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task CreateSettlement_ShouldAddNewSettlementToDatabase()
        {
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
			var controller = ControllerFactory.CreateSettlementController(context);
			var split = new Settlement { CountryId = croatia.Id, Name = "Split", PostalCode = "21000" };

            await controller.CreateSettlement(split);
            var settlementInDbWithNameSplit = await context.Settlements.FirstOrDefaultAsync(s => s.Name == split.Name);

            settlementInDbWithNameSplit.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteSettlement_ShouldReturnNotFound_WhenSettlementWithProvidedIdDoesntExist()
        {
            using var settlementContextFactory = new SettlementsContextFactory();
            using var context = settlementContextFactory.CreateContext();
			var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.DeleteSettlement(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteSettlement_ShouldReturnNoContent_WhenSettlementWasDeletetSuccessfully()
        {
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
			var controller = ControllerFactory.CreateSettlementController(context);

			var result = await controller.DeleteSettlement(zagreb.Id);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteSettlement_ShouldRemoveSettlementFromDatabase()
        {
			using var settlementContextFactory = new SettlementsContextFactory();
			using var context = settlementContextFactory.CreateContext();
			var croatia = await context.CreateAndAddCountryAsync(name: "Croatia");
			var zagreb = await context.CreateAndAddSettlementAsync(countryId: croatia.Id, name: "Zagreb");
			var controller = ControllerFactory.CreateSettlementController(context);

			await controller.DeleteSettlement(zagreb.Id);
            var settlementWithNameZagreb = await context.Settlements.FirstOrDefaultAsync(s => s.Name == zagreb.Name);

            settlementWithNameZagreb.Should().BeNull();
        }
    }
}
