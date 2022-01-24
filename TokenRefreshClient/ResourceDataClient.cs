using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleResourceOwnerFlowRefreshToken
{
    public static class ResourceDataClient
    {
        public static async Task GetDataAndDisplayInConsoleAsync(string access_token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.SetBearerToken(access_token);

            var payloadFromResourceServer = await httpClient.GetAsync("http://localhost:51306/api/v1/widget");
            if (!payloadFromResourceServer.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response unsuccessful: {payloadFromResourceServer.StatusCode}");
            }
            else
            {
                var content = await payloadFromResourceServer.Content.ReadAsStringAsync();
                Console.WriteLine($"Response successful: { content.ToString()}");
            }

        }
    }
}
