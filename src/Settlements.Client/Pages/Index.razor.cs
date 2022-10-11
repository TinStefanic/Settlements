using Microsoft.AspNetCore.Components;
using Settlements.Client.Services;
using Settlements.Shared.DTOs;

namespace Settlements.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private HostClient HostClient { get; set; } = null!;


        private IEnumerable<CountryDTO>? _countries;
        private PaginatedSettlementsDTO? _paginatedSettlements;

        private IEnumerable<CountryDTO> CountriesOrEmpty => _countries ?? Enumerable.Empty<CountryDTO>();


        protected override async Task OnInitializedAsync()
        {
            _countries = await HostClient.GetCountryDTOsAsync();
            _paginatedSettlements = await HostClient.GetPaginatedSettlementsDTOAsync();
            StateHasChanged();
        }

        private async Task RefreshPageAsync()
        {
            int? pageNumber = _paginatedSettlements?.PageNumber;
            int? pageSize = _paginatedSettlements?.PageSize;

            _paginatedSettlements = await HostClient.GetPaginatedSettlementsDTOAsync(pageNumber, pageSize);
            StateHasChanged();
        }
    }
}
