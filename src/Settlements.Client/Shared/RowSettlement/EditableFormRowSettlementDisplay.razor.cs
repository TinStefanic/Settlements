using Microsoft.AspNetCore.Components;
using Settlements.Shared.DTOs;

namespace Settlements.Client.Shared.RowSettlement
{
	public partial class EditableFormRowSettlementDisplay
	{
		[Parameter, EditorRequired]
		public SettlementDTO Settlement { get; set; } = null!;

		[Parameter, EditorRequired]
		public IEnumerable<CountryDTO> Countries { get; set; } = null!;

		[Parameter]
		public EventCallback OnCancelButtonClick { get; set; }


		private async Task HandleCancelButtonClick() => await OnCancelButtonClick.InvokeAsync();
	}
}
