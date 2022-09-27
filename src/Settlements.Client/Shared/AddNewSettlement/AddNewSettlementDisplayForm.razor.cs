using Microsoft.AspNetCore.Components;
using Settlements.Shared.DTOs;

namespace Settlements.Client.Shared.AddNewSettlement
{
	public partial class AddNewSettlementDisplayForm
	{
		[Parameter, EditorRequired]
		public IEnumerable<CountryDTO> Countries { get; set; } = null!;

		[Parameter, EditorRequired]
		public SettlementDTO Settlement { get; set; } = null!;

		[Parameter]
		public EventCallback OnCancelButtonClick { get; set; }


		private async Task HandleCancelButtonClick() => await OnCancelButtonClick.InvokeAsync();
	}
}
