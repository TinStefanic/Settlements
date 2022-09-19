using System.ComponentModel.DataAnnotations;

namespace Settlements.Client.DTOs
{
	public class SettlementDTO
	{
		public int Id { get; set; }

		[Required]
		public int? CountryId { get; set; }

		[Required]
		[StringLength(128, MinimumLength = 2)]
		public string? Name { get; set; }

		[Required]
		[StringLength(16, MinimumLength = 4)]
		public string? PostalCode { get; set; }
	}
}
