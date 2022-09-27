using System.ComponentModel.DataAnnotations;

namespace Settlements.Shared.DTOs
{
	public class SettlementDTO
	{
		public int Id { get; set; }

		public int? CountryId { get; set; }

		public string? Name { get; set; }

		public string? PostalCode { get; set; }
	}
}
