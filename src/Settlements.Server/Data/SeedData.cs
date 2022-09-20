using Microsoft.EntityFrameworkCore;
using Settlements.Server.Data.Models;

namespace Settlements.Server.Data
{
	public static class SeedData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using var context =
				new SettlementsContext(serviceProvider.GetRequiredService<DbContextOptions<SettlementsContext>>());

			if (context is null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			if (context.Settlements.Any())
			{
				return;
			}

			context.Countries.AddRange(
				new Country
				{
					Name = "Croatia",
					RegexPattern = "^\\d{5}$"
				},
				new Country
				{
					Name = "Japan",
					RegexPattern = "^\\d{3}\\p{Pd}?\\d{4}$" // p{Pd} is used because of these two ... '-', '‑'. 
				},
				new Country
				{
					Name = "USA",
					RegexPattern = "^\\b\\d{5}\\b(?:[\\p{Pd} ]{1}\\d{4})?$"
				}
			);

			context.SaveChanges();

			int croatiaId = context.Countries.FirstOrDefault(c => c.Name == "Croatia")?.Id ?? 1;
			int japanId = context.Countries.FirstOrDefault(c => c.Name == "Japan")?.Id ?? 1;
			int usaId = context.Countries.FirstOrDefault(c => c.Name == "USA")?.Id ?? 1;

			context.Settlements.AddRange(
				new Settlement
				{
					CountryId = croatiaId,
					Name = "Zagreb",
					PostalCode = "10000"
				},
				new Settlement
				{
					CountryId = croatiaId,
					Name = "Split",
					PostalCode = "21000"
				},
				new Settlement
				{
					CountryId = croatiaId,
					Name = "Rijeka",
					PostalCode = "51000"
				},
				new Settlement
				{
					CountryId = japanId,
					Name = "Tokyo",
					PostalCode = "100-0000"
				},
				new Settlement
				{
					CountryId = japanId,
					Name = "Kyoto",
					PostalCode = "600-0000"
				},
				new Settlement
				{
					CountryId = japanId,
					Name = "Osaka",
					PostalCode = "530-0000"
				},
				new Settlement
				{
					CountryId = usaId,
					Name = "New York",
					PostalCode = "10001"
				},
				new Settlement
				{
					CountryId = usaId,
					Name = "Chicago",
					PostalCode = "60601"
				}
			);

			context.SaveChanges();
		}
	}
}
