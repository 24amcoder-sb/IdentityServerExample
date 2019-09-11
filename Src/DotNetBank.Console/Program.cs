using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetBank.Console
{
    class Program
    {
        public static void Main() => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            //discover all the endpoints using metadata of identity server
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            if (disco.IsError)
            {
                WriteLine(disco.Error);
                return;
            }

            //Grab a bearer token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("DotNetBankApi");

            if (tokenResponse.IsError)
            {
                WriteLine(tokenResponse.Error);
                return;
            }

            WriteLine(tokenResponse.Json);
            WriteLine("\n\n");

            //Consume our Customer API
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var customerInfo = new StringContent(JsonConvert.SerializeObject(
                                                 new { Id= 1, FirstName = "Lokesh", LastName = "Swamy" }),
                                                 Encoding.UTF8, "application/json");

            var createCustomerResponse = await client.PostAsync("http://localhost:53857/api/customers", customerInfo);

            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                WriteLine(createCustomerResponse.StatusCode);
            }


            var getCustomerResponse = await client.GetAsync("http://localhost:53857/api/customers");

            if (!getCustomerResponse.IsSuccessStatusCode) {
                WriteLine(getCustomerResponse.StatusCode);
            }
            else {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                WriteLine(JArray.Parse(content));
            }

            Read();


        }
    }
}
