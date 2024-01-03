using Casino.SharedLibrary.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wallet.Infrastructure.Models.NowPayments;

namespace Wallet.Infrastructure.Services.NowPayments
{
    public sealed class NowPaymentsApiService : INowPaymentsApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly string _getFullCurrenciesUrl;
        private readonly string _getCheckedCurrenciesUrl;
        private readonly string _createPaymentUrl;

        public NowPaymentsApiService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _apiKey = configuration["NowPayments:ApiKey"];
            _getFullCurrenciesUrl = configuration["NowPayments:Endpoints:GetFullCurrencies"];
            _getCheckedCurrenciesUrl = configuration["NowPayments:Endpoints:GetCheckedCurrencies"];
            _createPaymentUrl = configuration["NowPayments:Endpoints:CreatePayment"];
        }

        public async Task<IEnumerable<CurrencyInfoDto>> GetEnabledCurrenciesAsync()
        {
            var httpClient = CreateHttpClient();

            var fullCurrenciesJsonResult = await HttpUtils.GetAsync(httpClient, _getFullCurrenciesUrl);
            var currenciesInfoDto = JsonConvert.DeserializeObject<GetFullCurrenciesDto>(fullCurrenciesJsonResult);

            var checkedCurrencies = await GetCheckedCurrenciesAsync();
            var enabledCurrencies = currenciesInfoDto.Currencies.Where(currency => checkedCurrencies.Contains(currency.Code));

            return enabledCurrencies;
        }

        public async Task<PaymentInfoDto> CreatePaymentAsync(CreatePaymentDto createPaymentDto)
        {
            var httpClient = CreateHttpClient();

            var requestBody = JsonConvert.SerializeObject(createPaymentDto);
            var content = new StringContent(requestBody);

            var resultString = await HttpUtils.PostAsync(httpClient, _createPaymentUrl, content);
            var paymentInfo = JsonConvert.DeserializeObject<PaymentInfoDto>(resultString);

            return paymentInfo;
        }

        private async Task<List<string>> GetCheckedCurrenciesAsync()
        {
            var httpClient = CreateHttpClient();

            var checkedCurrenciesJsonResult = await HttpUtils.GetAsync(httpClient, _getCheckedCurrenciesUrl);
            var jsonObject = JObject.Parse(checkedCurrenciesJsonResult);

            var checkedCurrenciesList = JsonConvert.DeserializeObject<List<string>>(jsonObject["selectedCurrencies"].ToString());

            return checkedCurrenciesList;
        }

        private HttpClient CreateHttpClient()
        {
			var httpClient = _httpClientFactory.CreateClient();
			httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

            return httpClient;
		}
    }
}