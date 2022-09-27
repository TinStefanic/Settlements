using Microsoft.AspNetCore.Components;
using Settlements.Shared.DTOs;

namespace Settlements.Client.Shared.AddNewSettlement.FieldsAsRows
{
	public partial class NameFieldAsRow
	{
		[Parameter, EditorRequired]
		public SettlementDTO Settlement { get; set; } = null!;
	}
}
