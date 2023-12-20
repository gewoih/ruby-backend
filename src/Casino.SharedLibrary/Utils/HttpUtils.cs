namespace Casino.SharedLibrary.Utils
{
    public static class HttpUtils
    {
        public const string JsonMediaType = "application/json";

        public static async Task<string> GetAsync(IHttpClientFactory httpClientFactory, string requestUrl)
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            return stringContent;
        }

        public static async Task<string> PostAsync(IHttpClientFactory httpClientFactory, string requestUrl, HttpContent content)
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.PostAsync(requestUrl, content);

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            return stringContent;
        }
    }
}
