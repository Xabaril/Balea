﻿using System.Text.Json;
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
#if NET5_0_OR_GREATER
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
#else 
            IgnoreNullValues = false,
#endif
            ReadCommentHandling = JsonCommentHandling.Disallow
        };

        public static async Task<TModel> GetJsonAsync<TModel>(this HttpClient client, string requestUri)
        {
            string json = await client.GetStringAsync(requestUri);
            return JsonSerializer.Deserialize<TModel>(json, _serializationOptions);
        }
    }
}
