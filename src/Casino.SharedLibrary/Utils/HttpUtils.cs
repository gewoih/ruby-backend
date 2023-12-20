using Newtonsoft.Json.Linq;

namespace Casino.SharedLibrary.Utils
{
    public static class HttpUtils
    {
        public const string JsonMediaType = "application/json";

        public static async Task<JObject> Get(IHttpClientFactory httpClientFactory, string requestUrl)
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            return JObject.Parse(stringContent);
        }

        public static async Task<JObject> Post(IHttpClientFactory httpClientFactory, string requestUrl, HttpContent content)
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.PostAsync(requestUrl, content);

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            return JObject.Parse(stringContent);
        }
    }
}
