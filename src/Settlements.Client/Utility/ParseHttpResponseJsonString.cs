using System.Text.Json;

namespace Settlements.Client.Utility
{
    public static class ParseHttpResponseErrorsAsDictiionaryExtension
    {
        public static async Task<Dictionary<string, IEnumerable<string>>> ReadAsErrorDictionaryAsync(
            this HttpContent response)
        {
            var jsonString = await response.ReadAsStringAsync();

            if (jsonString is null) return new Dictionary<string, IEnumerable<string>>();

            if (string.IsNullOrEmpty(jsonString)) return new Dictionary<string, IEnumerable<string>>();

            try
            {
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(jsonString);

                return jsonElement
                    .GetProperty("errors")
                    .Deserialize<Dictionary<string, IEnumerable<string>>>()
                    ?? new Dictionary<string, IEnumerable<string>>();
            }
            catch
            {
                return new Dictionary<string, IEnumerable<string>>();
            }
        }
    }
}
