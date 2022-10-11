using Microsoft.AspNetCore.Components;
using Settlements.Client.Services;
using Settlements.Shared.DTOs;

namespace Settlements.Client.Shared
{
    public partial class SettlementPaginatedTable
    {
        [Inject]
        HostClient HostClient { get; set; } = null!;


        [Parameter, EditorRequired]
        public IEnumerable<CountryDTO> Countries { get; set; } = null!;

        [Parameter]
        public PaginatedSettlementsDTO? PaginatedSettlements { get; set; }

        private bool HasPreviusPage => PaginatedSettlements?.PageNumber > 1;
        private bool HasNextPage
        {
            get
            {
                var ps = this.PaginatedSettlements;
                return ps?.TotalSettlementsCount > ps?.PageSize * ps?.PageNumber;
            }
        }


        public async Task LoadPreviousPage()
        {
            var currentPage = PaginatedSettlements?.PageNumber;
            var newPS = await HostClient.GetPaginatedSettlementsDTOAsync(pageNumber: currentPage - 1);
            UpdatePaginatedSettlements(newPS);
        }

        public async Task LoadNextPage()
        {
            var currentPage = PaginatedSettlements?.PageNumber;
            var newPS = await HostClient.GetPaginatedSettlementsDTOAsync(pageNumber: currentPage + 1);
            UpdatePaginatedSettlements(newPS);
        }

        public async Task ReloadPage()
        {
            var currentPage = PaginatedSettlements?.PageNumber;
            var newPS = await HostClient.GetPaginatedSettlementsDTOAsync(pageNumber: currentPage);
            UpdatePaginatedSettlements(newPS);
        }

        // To make parent's value update also.
        private void UpdatePaginatedSettlements(PaginatedSettlementsDTO? newPS)
        {
            if (PaginatedSettlements is null || newPS is null) return;
            PaginatedSettlements.Settlements = newPS.Settlements;
            PaginatedSettlements.PageNumber = newPS.PageNumber;
            PaginatedSettlements.TotalSettlementsCount = newPS.TotalSettlementsCount;
            StateHasChanged();
        }
    }
}
