using Microsoft.Extensions.Configuration;
using Settlements.Server.Controllers;
using Settlements.Server.Data;
using Settlements.Server.Services.ValidationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlements.UnitTests.Utility
{
	public static class ControllerFactory
	{
		public static SettlementsController CreateSettlementController(SettlementsContext context)
		{
			return new SettlementsController(
				context,
				new CustomSettlementValidationService(context),
				new ConfigurationBuilder().Build()
			);
		}

		public static CountriesController CreateCountriesController(SettlementsContext context)
		{
			return new CountriesController(context);
		}
	}
}
