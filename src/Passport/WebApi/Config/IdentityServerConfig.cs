using IdentityServer4;
using IdentityServer4.Models;

namespace Casino.Passport.Config
{
	public class IdentityServerConfig
	{
		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId()
			};
		}

		public static IEnumerable<ApiScope> GetApiScopes()
		{
			return new List<ApiScope>
			{
				new("all", "Full Casino API")
			};
		}

		public static IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
			{
				new("casino-api", "Casino API")
				{
					Scopes = { "all" }
				}
			};
		}

		public static IEnumerable<Client> GetClients()
		{
			return new List<Client>
			{
				new()
				{
					ClientId = "web_app",
					ClientName = "Casino WebApp",
					AllowedGrantTypes = GrantTypes.Code,

					ClientSecrets =
					{
						new Secret("A43DCC44-AC8C-46EE-B312-ECA6F6CBFA52".Sha256())
					},

					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId, "all"
					},
					RequirePkce = true,
					RedirectUris = { "https://localhost:7220/login/callback" },
					AllowOfflineAccess = true,
				}
			};
		}
	}
}
