using Casino.SharedLibrary.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wallet.Infrastructure.Models.NowPayments;

namespace Wallet.Infrastructure.Services.NowPayments
{
    public sealed class NowPaymentsService : INowPaymentsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly string _getFullCurrenciesUrl;
        private readonly string _getCheckedCurrenciesUrl;

        public NowPaymentsService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _apiKey = configuration["NowPayments:ApiKey"];
            _getFullCurrenciesUrl = configuration["NowPayments:Endpoints:GetFullCurrencies"];
            _getCheckedCurrenciesUrl = configuration["NowPayments:Endpoints:GetCheckedCurrencies"];
        }

        public async Task<IEnumerable<CurrencyInfo>> GetEnabledCurrencies()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

            var fullCurrenciesJsonResult = await HttpUtils.GetAsync(httpClient, _getFullCurrenciesUrl);
            var currenciesInfoDto = JsonConvert.DeserializeObject<GetFullCurrenciesDto>(fullCurrenciesJsonResult);

            var checkedCurrencies = await GetCheckedCurrencies();
            var enabledCurrencies = currenciesInfoDto.Currencies.Where(currency => checkedCurrencies.Contains(currency.Code));

            return enabledCurrencies;
        }

        private async Task<List<string>> GetCheckedCurrencies()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

            var checkedCurrenciesJsonResult = await HttpUtils.GetAsync(httpClient, _getCheckedCurrenciesUrl);
            var jsonObject = JObject.Parse(checkedCurrenciesJsonResult);

            var checkedCurrenciesList = JsonConvert.DeserializeObject<List<string>>(jsonObject["selectedCurrencies"].ToString());

            return checkedCurrenciesList;
        }
    }
}