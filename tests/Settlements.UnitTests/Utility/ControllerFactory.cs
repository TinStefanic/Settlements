using Microsoft.Extensions.Configuration;
using Settlements.Server.Controllers;
using Settlements.Server.Data;
using Settlements.Server.Validators;

namespace Settlements.UnitTests.Utility
{
    public static class ControllerFactory
    {
        public static SettlementsController CreateSettlementController(SettlementsContext context)
        {
            return new SettlementsController(
                context,
                new ConfigurationBuilder().Build(),
                new SettlementDTOServerValidator(context)
            );
        }

        public static CountriesController CreateCountriesController(SettlementsContext context)
        {
            return new CountriesController(context);
        }
    }
}
