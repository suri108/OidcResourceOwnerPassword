using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleResourceOwnerFlowRefreshToken
{
    public static class IdentityServer4Client
    {
        private static TokenClient _tokenClient;

        public static async Task<TokenResponse> LoginAsync(string user, string password)
        {
            Console.Title = "Console ResourceOwner Flow RefreshToken";

            var disco = await DiscoveryClient.GetAsync("http://localhost:51310");
            if (disco.IsError) throw new Exception(disco.Error);

            _tokenClient = new TokenClient(
                disco.TokenEndpoint,
                "resourceownerclient",
                "dataEventRecordsSecret");

            return await RequestTokenAsync(user, password);
        }

        public static async Task RunRefreshAsync(TokenResponse response, int milliseconds)
        {
            var refresh_token = response.RefreshToken;

            while (true)
            {
                response = await RefreshTokenAsync(refresh_token);

                // Get the resource data using the new tokens...
                await ResourceDataClient.GetDataAndDisplayInConsoleAsync(response.AccessToken);

                if (response.RefreshToken != refresh_token)
                {
                    ShowResponse(response);
                    refresh_token = response.RefreshToken;
                }

                Task.Delay(milliseconds).Wait();
            }
        }
        private static async Task<TokenResponse> RequestTokenAsync(string user, string password)
        {
            Console.Write("begin RequestTokenAsync");
            return await _tokenClient.RequestResourceOwnerPasswordAsync(
                user,
                password,
                "openid profile dataEventRecords offline_access");
        }

        private static async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            Console.Write($"Using refresh token: {refreshToken}");

            return await _tokenClient.RequestRefreshTokenAsync(refreshToken);
        }

        private static void ShowResponse(TokenResponse response)
        {
            if (!response.IsError)
            {
                Console.Write($"Token response: {response.Json.ToString()}");

                if (response.AccessToken.Contains("."))
                {
                    var parts = response.AccessToken.Split('.');
                    var header = parts[0];
                    var claims = parts[1];

                    Console.Write($"Access Token Header decoded {JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(header))).ToString()}");
                    Console.Write($"Access Token claims decoded {JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims))).ToString()} ");
                }
            }
            else
            {
                if (response.ErrorType == ResponseErrorType.Http)
                {
                    Console.Write("HTTP error:  {ResponseError}", response.Error);
                    Console.Write("HTTP status code:  {ResponseHttpStatusCode}", response.HttpStatusCode);
                }
                else
                {
                    Console.Write("Protocol error response: {ResponsePayload}", response.Json);
                }
            }
        }
    }
}
