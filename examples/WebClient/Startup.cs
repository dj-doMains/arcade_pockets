using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebClient
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
            services.AddJwtManager()
                .AddInMemoryStore(options =>
                {
                    options.GrantType = ArcadePocketsGrantType.Password;
                    options.TokenIssuerUri = "http://localhost:50000/connect/token";
                    options.ExpirationCushion = 3700;
                    options.ClientID = "2b11357c-668a-4627-b956-c9ad1365c8b3";
                    options.ClientSecret = "bbda4042-7d7a-4e41-9d8e-27c3ab4a8260";
                    options.Username = "greendiggle@arcadepockets.com";
                    options.Password = "arr0wWho?";
                });

            services.AddMvc();
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
