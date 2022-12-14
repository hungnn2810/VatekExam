using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityService.Commons.Constants;

namespace IdentityService
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("main.read_write", "Full access to microservices")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
               new ApiResource()
               {
                   Name = "microservices",
                   DisplayName = "Microservice",
                   ApiSecrets = { new Secret(AppSettingConstants.IdentityServer.ApiSecret.Sha256()) },
                   Enabled = true,
                   Scopes = { "main.read_write" }
               }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = AppSettingConstants.IdentityServer.ClientId,
                    ClientSecrets = { new Secret (AppSettingConstants.IdentityServer.ClientSecret.Sha256()) },
                    AllowedGrantTypes = new List<string> { GrantType.ClientCredentials, GrantType.ResourceOwnerPassword },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "main.read_write" },
                    AccessTokenType = AccessTokenType.Reference,
                    AllowOfflineAccess = true,
                    RequireClientSecret = true,
                }
            };
    }
}

