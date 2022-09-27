namespace Settlements.Shared.DTOs
{
	public class PaginatedSettlementsDTO
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public IEnumerable<SettlementDTO> Settlements { get; set; } = Enumerable.Empty<SettlementDTO>();
		public int TotalSettlementsCount { get; set; }
	}
}
