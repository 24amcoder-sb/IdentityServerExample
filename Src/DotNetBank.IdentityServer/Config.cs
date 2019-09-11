using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBank.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("DotNetBankApi", "Customer Api for DotNetBank")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "DotNetBankApi" }
                }
            };
        }
    }
}
