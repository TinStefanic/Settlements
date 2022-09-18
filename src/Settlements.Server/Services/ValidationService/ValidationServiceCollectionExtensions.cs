using Settlements.Server.Services.ValidationService;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ValidationServiceCollectionExtensions
	{
		public static IServiceCollection AddValidationServices(
			 this IServiceCollection services)
		{
			services.AddScoped<ICustomSettlementValidationService, CustomSettlementValidationService>();

			return services;
		}
	}
}
