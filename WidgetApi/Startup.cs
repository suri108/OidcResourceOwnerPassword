using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace WidgetApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationUrl = "http://localhost:51310";

            //services.AddMvcCore()
            //    .AddAuthorization()
            //    .AddJsonFormatters();

            //services.AddAuthentication("Bearer")
            //        .AddIdentityServerAuthentication(options =>
            //        {
            //            options.Authority = authenticationUrl; // Auth Server
            //            options.RequireHttpsMetadata = false;
            //            options.ApiName = "widgetapi"; // API Resource Id
            //        });

            //var guestPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .RequireClaim("scope", "dataEventRecords")
            //    .Build();

            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //        .AddIdentityServerAuthentication(options =>
            //        {
            //            options.Authority = authenticationUrl; // Auth Server
            //            options.ApiName = "dataEventRecords";
            //            options.ApiSecret = "dataEventRecordsSecret";
            //            options.RequireHttpsMetadata = false;
            //        });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("dataEventRecordsAdmin", policyAdmin =>
            //    {
            //        policyAdmin.RequireClaim("role", "dataEventRecords.admin");
            //    });
            //    options.AddPolicy("dataEventRecordsUser", policyUser =>
            //    {
            //        policyUser.RequireClaim("role", "dataEventRecords.user");
            //    });
            //    options.AddPolicy("dataEventRecords", policyUser =>
            //    {
            //        policyUser.RequireClaim("scope", "dataEventRecords");
            //    });
            //});

            services.AddMvc(
            //    options =>
            //{
            //    options.Filters.Add(new AuthorizeFilter(guestPolicy));
            //}
            ).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

          //  app.UseHttpsRedirection();
           // app.UseAuthentication();
            app.UseMvc();
        }
    }
}
