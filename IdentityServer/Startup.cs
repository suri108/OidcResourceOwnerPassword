using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QuickstartIdentityServer;

namespace IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        

        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;
        private readonly ILoggerFactory _loggerFactory;
        public KeyParameters _keyParameters;

        public Startup(IHostingEnvironment env, IConfiguration config, ILoggerFactory loggerFactory)
        {
            _env = env;
            _config = config;
            _loggerFactory = loggerFactory;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // configure identity server with in-memory stores, keys, clients and scopes
            services
                .AddIdentityServer()
                .AddDeveloperSigningCredential(filename: "tempkey.rsa")
                //.AddAspNetIdentity<ApplicationUser>()
                //.AddSigningCredential(CreateSigningCredentials())
                //    .AddInMemoryApiResources(Config.GetApiResources())
                //    .AddInMemoryClients(Config.GetClients());
                //.AddSigningCredential(cert)
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddCustomUserStore();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }

        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.RsaSha256Signature);
        }

        private SecurityKey GetSecurityKey()
        {
           // { "KeyId":"6ca39c3dd4ffda97d502243e25fa4e54","Parameters":{ "D":"k0xLrR3P/XP9UC/XdUTRFiikbZHmQyQ1RS2qAZiCz9m81RglsEjxrbA4FBfWisk6Sq15L7F1E0iaY65gBWIgb1aJDXobUT0ky2TMCd1T1eprx1Ey4z/8Nn1NSjvZ77b1hqucbJAXds+1dKHSMCyx3hQB517OMFEw/e5nb+VshfuZQ7cjEYhCPW4R/TFAcfq2QtfjpeWz5Br62nkazVOeAkYZC3/rTclhRXWjeTIUSaw0mLG4fRLfbyBG78KlZvm4Bqbnk0XhsD/zwwGZ5Wrj7hWC/kUNi5qdUjVfyNL/RIUn1YXURmimKY6pE2o1t2VJVgs0q60psYPVQu51FYmuDQ==","DP":"DjEVGoT+9sO0C9qjizuDHeus3ahmrIzrSMR/KPBaqPcpG1zlaX/grxLQGmZmdfMHWsb8mtxJ482eRYnnmRPFt/bpfxI60IS536GdkBmKCgQzZSQrkw3frq4v++GS/uPPla61YNrTuWExkd2AriKinhiTsKPwx5bywTv8EaeDQ60=","DQ":"PpTdIaGCNWXnX9zE1bIC0pdBpHCtRKb/b81GbE3MYnahyjYNI+uZ/nT/7e9pyG76+/0NXgVO7mdWpcOJJGVID+6SvgTqnPHQ2EIcxCYl/HxjssI480OEmdSZCnDFPfPyOybuRZO1CyzKNsHFu57BGpg7R/mf8PGvAv1YXfLMKzE=","Exponent":"AQAB","InverseQ":"gAJfxokhPHqSYaY9N1j0JBTa+2fQDNapyFsDRoMOsT0eu6Ls+9kkh+xNTrN8E2RqIxGfYu4J1HuhvLeXjEKW8v/xbDcRJzY+1ptrlQiUVKVEfn/QCtgUQiFfpEi26ooHkyTw2YCTxJBITr2+RPV72vx6MU4wL+iN+ky0N3QZZeU=","Modulus":"sZthlS0HE1pkbSnMlPyKNDkAqkQryeKG7YSRMeUbrDQARu+9f11iUFUblAdXUhuFRu0R77AQ+mhjy7kfjQMOT58gp3aMa17HTKcMxZRZEi+zcXZuxVA7Q0nuWrWp4/+0VAMV4OhGromZCFtUb26kRJXyKMNlHSM2irSJ9LWnx6NtSkHMrC/kv3kpciZWLx//9DkVM7wmYuGz9DMezoz7+FuwcJcGJHmVz7RNRwGNhdcvEG8nJE3fl8QQ16CjOim2X845gaIc9dWKi1MAA/LS1M2EK4aU8FZjVqgQgY472zrwGtUtwz25aUEZu130fthZabvOiWTDbztuYtOmrxP7BQ==","P":"zPRW2lJyKljWyzyNlpFzgDbDtrw1++pUCXcGvNd5Ir1v87PGJKDJOH84Xjy/mLx9yfnfYovyA87s4pAHY5bHdbZLeqAXn2VWyFky+xkOE/vK0oHKBqAj6N0PzKPCMrQ1VnDFpRMZn3k6RwcGouF3heC8QsWGH1maPfuPfjpFAM8=","Q":"3ddlq5IqMKGvqFLspgj623kinOON+dsO08FjRp8xTF2NeIVAuDp1yCfbQ3PdxL8FzSetEolReH+upr71zstVQgCzoQ+9HjEnPi97KEBju8u3iqk0QDNJaCUJ2HqDI6+UiUQJCQAJSiIxdF5U0TRZ21okHYix1Cfg04dpuXr9M+s="} }
             return new RsaSecurityKey(GetRSAParameters());
        }

        private RSAParameters GetRSAParameters()
        {
            /*
                Secret Manager is used for this project. But I did not need to call 
                AddUserSecrets<Startup>() above.
            
                In ASP.NET Core 2.0 or later, the user secrets configuration source is automatically 
                added in development mode when the project calls CreateDefaultBuilder to initialize 
                a new instance of the host with preconfigured defaults. CreateDefaultBuilder calls 
                AddUserSecrets when the EnvironmentName is Development.

                In production the secrets will not be deployed so the app settings would have to be
                set at deploy time.
             */
            //var keyParameters = Configuration.GetSection("KeyParameters").Get<KeyParameters>();
            return new RSAParameters
            {
                D = Base64UrlEncoder.DecodeBytes("1234"),
                DP = Base64UrlEncoder.DecodeBytes("1234"),
                DQ = Base64UrlEncoder.DecodeBytes("1234"),
                Exponent = Base64UrlEncoder.DecodeBytes("1234"),
                InverseQ = Base64UrlEncoder.DecodeBytes("1234"),
                Modulus = Base64UrlEncoder.DecodeBytes("1234"),
                P = Base64UrlEncoder.DecodeBytes("1234"),
                Q = Base64UrlEncoder.DecodeBytes("1234")
            };

            //return new RSAParameters
            //{
            //    //D = Base64UrlEncoder.DecodeBytes("http://localhost"),
            //    //DP = Base64UrlEncoder.DecodeBytes("http://localhost"),
            //    //DQ = Base64UrlEncoder.DecodeBytes("http://localhost"),
            //    //Exponent = Base64UrlEncoder.DecodeBytes("http://localhost"),
            //    //InverseQ = Base64UrlEncoder.DecodeBytes("http://localhost"),
            //    //Modulus = Base64UrlEncoder.DecodeBytes("http://localhost"),
            //    //P = Base64UrlEncoder.DecodeBytes("http://localhost"),
            //    //Q = Base64UrlEncoder.DecodeBytes("http://localhost")
            //    D = Base64UrlEncoder.DecodeBytes("1234"),
            //    Exponent = Base64UrlEncoder.DecodeBytes("1234"),
            //    Modulus = Base64UrlEncoder.DecodeBytes("1234")
            //};
        }
    }
}
