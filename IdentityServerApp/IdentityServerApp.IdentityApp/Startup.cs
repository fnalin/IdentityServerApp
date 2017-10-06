using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using IdentityServerApp.IdentityApp.Infra;

namespace IdentityServerApp.IdentityApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentityServer()
                //.AddTemporarySigningCredential() .net 1.1
                .AddDeveloperSigningCredential() //.net core 2
                .AddInMemoryApiResources(ConfigIdentity.GetApiResources())
                .AddInMemoryClients(ConfigIdentity.GetClients());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Not Found parceiro ;)!");
            });
        }
    }
}
