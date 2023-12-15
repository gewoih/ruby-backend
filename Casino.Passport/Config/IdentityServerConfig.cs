using IdentityServer4;
using IdentityServer4.Models;

namespace Casino.Passport.Config
{
	public class IdentityServerConfig(IConfiguration configuration)
	{
		public IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId()
			};
		}

		public IEnumerable<ApiScope> GetApiScopes()
		{
			return new List<ApiScope>
			{
				new("all", "Full Casino API")
			};
		}

		public IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
			{
				new("casino-api", "Casino API")
				{
					Scopes = { "all" }
				}
			};
		}

		public IEnumerable<Client> GetClients()
		{	
			var redirectUris = configuration.GetValue<string>("RedirectUris")?.Split(";").ToList();
			var allowedCors = configuration.GetValue<string>("AllowedCors")?.Split(";").ToList();

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
					PostLogoutRedirectUris = redirectUris,
					AllowedCorsOrigins = allowedCors,
					AllowAccessTokensViaBrowser = true
				}
			};
		}
	}
}
