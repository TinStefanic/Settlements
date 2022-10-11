using Microsoft.AspNetCore.Components;
using Settlements.Shared.DTOs;

namespace Settlements.Client.Shared.RowSettlement
{
    public partial class ReadonlyRowSettlementDisplay
    {
        [Parameter, EditorRequired]
        public IEnumerable<CountryDTO> Countries { get; set; } = null!;

        [Parameter, EditorRequired]
        public SettlementDTO Settlement { get; set; } = null!;

        [Parameter]
        public EventCallback OnEditButtonClick { get; set; }

        [Parameter]
        public EventCallback OnDeleteButtonClick { get; set; }

        private string SettlementsCountryName => Countries.FirstOrDefault(c => c.Id == Settlement.CountryId)?.Name!;


        private async Task HandleEditButtonClick() => await OnEditButtonClick.InvokeAsync();

        private async Task HandleDeleteButtonClick() => await OnDeleteButtonClick.InvokeAsync();
    }
}
