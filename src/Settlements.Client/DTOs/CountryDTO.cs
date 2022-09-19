using System.ComponentModel.DataAnnotations;

namespace Settlements.Client.DTOs
{
	public class CountryDTO
	{
		public int Id { get; set; }

		[Required]
		[StringLength(128, MinimumLength = 2)]
		public string? Name { get; set; }

		[Required]
		public string? RegexPattern { get; set; }
	}
}
