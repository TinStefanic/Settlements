using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Settlements.Shared.DTOs;

namespace Settlements.Client.Services
{
    /// <summary>
    /// Http client to access Settlements.Server api that hosted the client.
    /// </summary>
    public class HostClient
    {
        private const string _countriesApiUri = "api/Countries";
        private const string _settlementsApiUri = "api/Settlements";
        private readonly HttpClient _httpClient;

        public HostClient(HttpClient httpClient, IWebAssemblyHostEnvironment hostEnvironment)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(hostEnvironment.BaseAddress);
        }

        public async Task<IEnumerable<CountryDTO>?> GetCountryDTOsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CountryDTO>>(_countriesApiUri);
        }

        /// <summary>
        /// Fetches PaginatedSettlements from api, if any parameter is null then it uses servers defaults.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PaginatedSettlementsDTO?> GetPaginatedSettlementsDTOAsync(
            int? pageNumber = null,
            int? pageSize = null)
        {

            var queryString = new Dictionary<string, string>();

            if (pageNumber is not null)
            {
                queryString.Add("pageNumber", pageNumber?.ToString()!);
            }
            if (pageSize is not null)
            {
                queryString.Add("pageSize", pageSize?.ToString()!);
            }

            string url = QueryHelpers.AddQueryString(_settlementsApiUri, queryString);

            return await _httpClient.GetFromJsonAsync<PaginatedSettlementsDTO>(url);
        }

        public async Task<SettlementDTO?> GetSettlementDTOAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<SettlementDTO>($"{_settlementsApiUri}/{id}");
        }

        public async Task<HttpResponseMessage> UpdateSettlementDTOAsync(int id, SettlementDTO settlementDTO)
        {
            return await _httpClient.PutAsJsonAsync($"{_settlementsApiUri}/{id}", settlementDTO);
        }

        public async Task<HttpResponseMessage> CreateSettlementDTOAsync(SettlementDTO settlementDTO)
        {
            return await _httpClient.PostAsJsonAsync(_settlementsApiUri, settlementDTO);
        }

        public async Task<HttpResponseMessage> DeleteSettlementDTOAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{_settlementsApiUri}/{id}");
        }
    }
}
