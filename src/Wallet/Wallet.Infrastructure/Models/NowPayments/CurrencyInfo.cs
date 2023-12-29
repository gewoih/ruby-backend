using Newtonsoft.Json;

namespace Wallet.Infrastructure.Models.NowPayments
{
	public sealed class CurrencyInfo
	{
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        
        [JsonProperty("enable")]
        public bool Enabled { get; set; }

        [JsonProperty("wallet_regex")]
        public string WalletRegex { get; set; }
        
        public int Priority { get; set; }

        [JsonProperty("extra_id_exists")]
        public bool ExtraIdExists { get; set; }

        [JsonProperty("extra_id_regex")]
        public string ExtraIdRegex { get; set; }

        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }
        
        public bool Track { get; set; }

        [JsonProperty("cg_id")]
        public string CgId { get; set; }

        [JsonProperty("is_maxlimit")]
        public bool IsMaxLimit { get; set; }
        public string Network { get; set; }

        [JsonProperty("smart_contract")]
        public string SmartContract { get; set; }

        [JsonProperty("network_precision")]
        public string NetworkPrecision { get; set; }
	}
}
