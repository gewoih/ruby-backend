namespace Casino.SharedLibrary.Utils
{
    public static class HttpUtils
    {
        public const string JsonMediaType = "application/json";

        public static async Task<string> GetAsync(HttpClient httpClient, string requestUrl)
        {
            var response = await httpClient.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            return stringContent;
        }

        public static async Task<string> PostAsync(HttpClient httpClient, string requestUrl, HttpContent content)
        {
            var response = await httpClient.PostAsync(requestUrl, content);
            var stringContent = await response.Content.ReadAsStringAsync();
            
            response.EnsureSuccessStatusCode();
            
            return stringContent;
        }
    }
}
