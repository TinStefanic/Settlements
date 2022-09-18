using System.ComponentModel.DataAnnotations;

namespace Settlements.Shared.Contracts
{
	public class SettlementDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		[StringLength(128, MinimumLength = 2)]
		public string? Country { get; set; }

		[Required]
		[StringLength(128, MinimumLength = 2)]
		public string? Name { get; set; }

		[Required]
		[StringLength(16, MinimumLength = 4)]
		public string? PostalCode { get; set; }
	}
}
