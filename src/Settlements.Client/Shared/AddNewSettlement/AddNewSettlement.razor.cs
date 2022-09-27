using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Settlements.Shared.DTOs;
using Settlements.Client.Utility;
using Settlements.Client.Services;

namespace Settlements.Client.Shared.AddNewSettlement
{
	public partial class AddNewSettlement
	{
		[Inject]
		private HostClient HostClient { get; set; } = null!;


		[Parameter, EditorRequired]
		public IEnumerable<CountryDTO> Countries { get; set; } = null!;
		[Parameter]
		public EventCallback OnSuccessfullyAdded { get; set; }

		private bool _isResponseStatusSuccess = false;
		private bool _shouldDisplayForm = false;

		private SettlementDTO Settlement { get; set; } = new SettlementDTO();

		//Properly initialized in OnInitialized method.
		private EditContext? _editContext;
		private ValidationMessageStore? _validationMessageStore;


		protected override void OnInitialized()
		{
			_editContext = new EditContext(Settlement);
			_validationMessageStore = new ValidationMessageStore(_editContext);
			_editContext.OnFieldChanged += FieldChangedHandler;
		}

		private void HandleCreateNewButtonClick()
		{
			_shouldDisplayForm = true;
			_isResponseStatusSuccess = false;
		}

		private void HandleCancelButtonClick()
		{
			_shouldDisplayForm = false;
			Settlement = new SettlementDTO();
			_isResponseStatusSuccess = false;

			_editContext!.OnFieldChanged -= FieldChangedHandler;
			_editContext = new EditContext(Settlement);
			_validationMessageStore!.Clear();
			_validationMessageStore = new ValidationMessageStore(_editContext);
			_editContext.OnFieldChanged += FieldChangedHandler;

			StateHasChanged();
		}

		private async Task HandleSubmit()
		{
			_isResponseStatusSuccess = false;
			_validationMessageStore!.Clear();

			if (_editContext!.Validate())
			{
				await HandleValidSubmit();
			}
		}

		private async Task HandleValidSubmit()
		{
			var response = await HostClient.CreateSettlementDTOAsync(Settlement);
			_isResponseStatusSuccess = response.IsSuccessStatusCode;

			if (response.IsSuccessStatusCode)
			{
				Settlement = new SettlementDTO();
				_shouldDisplayForm = false;
				await OnSuccessfullyAdded.InvokeAsync();
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
		private void FieldChangedHandler(object? o, FieldChangedEventArgs e)
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
