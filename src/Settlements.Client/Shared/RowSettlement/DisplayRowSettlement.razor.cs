using Mapster;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Settlements.Shared.DTOs;
using Settlements.Client.Utility;
using Settlements.Client.Services;

namespace Settlements.Client.Shared.RowSettlement
{
	public partial class DisplayRowSettlement
	{
		[Inject] 
		IJSRuntime JsRuntime { get; set; } = null!;
		[Inject] 
		HostClient HostClient { get; set; } = null!;


		[Parameter, EditorRequired]
		public IEnumerable<CountryDTO> Countries { get; set; } = null!;

		[Parameter, EditorRequired]
		public SettlementDTO Settlement { get; set; } = null!;

		[Parameter]
		public EventCallback OnChange { get; set; }

		private SettlementDTO? _settlementCopy;

		//Properly initialized in OnParametersSet method.
		private EditContext? _editContext;
		private ValidationMessageStore? _validationMessageStore;

		private bool _readOnly = true;


		protected override void OnParametersSet()
		{
			_readOnly = true;
			_editContext = new EditContext(Settlement);
			_validationMessageStore = new ValidationMessageStore(_editContext);
			_editContext.OnFieldChanged += FieldChangedHandler;

			_settlementCopy = Settlement.Adapt<SettlementDTO>();
		}

		private void HandleEdit()
		{
			_readOnly = false;
			StateHasChanged();
		}

		private async Task HandleDelete()
		{
			var response = await HostClient.DeleteSettlementDTOAsync(Settlement.Id);

			if (response.IsSuccessStatusCode)
			{
				await OnChange.InvokeAsync();
			}
			else
			{
				await JsRuntime.InvokeVoidAsync("alert", "Delete failed!");
			}

			StateHasChanged();
		}

		private void HandleCancelButtonClick()
		{
			_readOnly = true;
			_settlementCopy!.Adapt(Settlement);

			_editContext!.OnFieldChanged -= FieldChangedHandler;
			_editContext = new EditContext(Settlement);
			_validationMessageStore!.Clear();
			_validationMessageStore = new ValidationMessageStore(_editContext);
			_editContext.OnFieldChanged += FieldChangedHandler;

			StateHasChanged();
		}

		private async Task HandleSubmit()
		{
			_validationMessageStore!.Clear();

			if (_editContext!.Validate())
			{
				await HandleValidSubmit();
			}
		}

		private async Task HandleValidSubmit()
		{
			var response = await HostClient.UpdateSettlementDTOAsync(Settlement.Id, Settlement);

			if (response.IsSuccessStatusCode)
			{
				_readOnly = true;
			}
			else
			{
				var errorDictionary = await response.Content.ReadAsErrorDictionaryAsync();

				errorDictionary
					.ToList()
					.ForEach(p => _validationMessageStore!.Add(new FieldIdentifier(Settlement, p.Key), p.Value));
			}
		}

		// Once form field has changed, its server side validation message should be removed.
		private void FieldChangedHandler(Object? o, FieldChangedEventArgs e)
		{
			var vms = new ValidationMessageStore(_editContext!);

			Settlement
				.GetType()
				.GetProperties()
				.Select(pi => new FieldIdentifier(Settlement, pi.Name))
				.Where(fi => fi.FieldName != e.FieldIdentifier.FieldName) // Ignores field that was just changed.
				.ToList()
				.ForEach(fi =>
					vms.Add(fi, _validationMessageStore![fi])
				);

			_validationMessageStore!.Clear();
			_validationMessageStore = vms;

			StateHasChanged();
		}
	}
}
