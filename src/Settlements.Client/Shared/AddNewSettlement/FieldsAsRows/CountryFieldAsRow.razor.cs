using Microsoft.AspNetCore.Components;
using Settlements.Shared.DTOs;

namespace Settlements.Client.Shared.AddNewSettlement.FieldsAsRows
{
	public partial class CountryFieldAsRow
	{
		[Parameter, EditorRequired]
		public IEnumerable<CountryDTO> Countries { get; set; } = null!;

		[Parameter, EditorRequired]
		public SettlementDTO Settlement { get; set; } = null!;
	}
}
