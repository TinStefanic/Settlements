using Settlements.Server.Data.Models;

namespace Settlements.Server.DTOs
{
	public class PaginatedSettlements
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public IEnumerable<Settlement> Settlements { get; set; } = Enumerable.Empty<Settlement>();
		public int TotalSettlementsCount { get; set; }
	}
}
