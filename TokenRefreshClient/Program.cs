//using Serilog;
using System.Threading.Tasks;

namespace ConsoleResourceOwnerFlowRefreshToken
{
    public class Program
    {
        
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
           
            var response = IdentityServer4Client.LoginAsync("damienbod", "damienbod").Result;


            // GET DATA from the resource server
            await ResourceDataClient.GetDataAndDisplayInConsoleAsync(response.AccessToken);


            // Run an loop which gets refreshes the token every 3000 milliseconds
            await IdentityServer4Client.RunRefreshAsync(response, 300000);
        }
    }
}