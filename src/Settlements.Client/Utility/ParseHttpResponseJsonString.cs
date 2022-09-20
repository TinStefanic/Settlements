using System.Text.Json;

namespace Settlements.Client.Utility
{
    public class ParseHttpResponseJsonString
    {
        private readonly string? _jsonString;

        public ParseHttpResponseJsonString(string? jsonString)
        {
            _jsonString = jsonString;
        }

        public IEnumerable<string> ToIEnumerableString()
        {
			if (_jsonString is null) return Enumerable.Empty<string>();

			var result = new List<string>();

			if (string.IsNullOrEmpty(_jsonString)) return Enumerable.Empty<string>();

			var jsonElement = JsonSerializer.Deserialize<JsonElement>(_jsonString);

			try
			{
				var errorsProperties = jsonElement.GetProperty("errors").EnumerateObject();
				foreach (var propertyErrors in errorsProperties)
				{
					foreach (var error in propertyErrors.Value.EnumerateArray())
					{
						string? errorAsString = error.GetString();
						if (errorAsString is not null)
						{
							result.Add(errorAsString);
						}
					}
				}
			}
			catch
			{
				return Enumerable.Empty<string>();
			}

			return result;
		}
    }
}
