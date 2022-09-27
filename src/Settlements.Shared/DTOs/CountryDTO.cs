using System.ComponentModel.DataAnnotations;

namespace Settlements.Shared.DTOs
{
	public class CountryDTO
	{
		public int Id { get; set; }

		public string? Name { get; set; }

		public string? RegexPattern { get; set; }
	}
}
