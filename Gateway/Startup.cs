using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

namespace Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationUrl = "http://localhost:51310";
            //services.AddAuthentication("Bearer")
            //       .AddIdentityServerAuthentication(options =>
            //       {
            //           options.Authority = authenticationUrl; // Auth Server
            //            options.RequireHttpsMetadata = false;
            //           options.ApiName = "widgetapi"; // API Resource Id
            //           options.SupportedTokens = SupportedTokens.Both;
            //       });

            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.Authority = authenticationUrl;
                o.ApiName = "dataEventRecords";
                o.ApiSecret = "dataEventRecordsSecret";
                o.SupportedTokens = SupportedTokens.Both;
                //o.ApiSecret = "secret";
                o.RequireHttpsMetadata = false;
            };

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("TestKey", options);
            services.AddOcelot(Configuration);
        }

        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            // app.UseMvc();

            await app.UseOcelot();
        }
    }
}
