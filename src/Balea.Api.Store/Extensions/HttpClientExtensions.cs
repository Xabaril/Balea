using System.Text.Json;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static JsonSerializerOptions _serializationOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = false,
            ReadCommentHandling = JsonCommentHandling.Disallow
        };

        public static async Task<TModel> GetJsonAsync<TModel>(this HttpClient client, string requestUri)
        {
            string json = await client.GetStringAsync(requestUri);
            return JsonSerializer.Deserialize<TModel>(json, _serializationOptions);
        }
    }
}
