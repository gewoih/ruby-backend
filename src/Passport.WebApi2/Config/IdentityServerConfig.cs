using Duende.IdentityServer;
using Duende.IdentityServer.Models;

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

		public static IEnumerable<Client> GetClients(IConfiguration configuration)
		{	
			var redirectUris = configuration.GetValue<string>("RedirectUris")?.Split(";").ToList();

			return new List<Client>
			{
				new()
				{
					ClientId = "web_app",
					ClientName = "Casino WebApp",
					AllowedGrantTypes = GrantTypes.Code,
					RequireClientSecret = false,
					
					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId, "all"
					},
					RedirectUris = redirectUris,
					RequirePkce = true,
					AllowAccessTokensViaBrowser = true
				}
			};
		}
	}
}
