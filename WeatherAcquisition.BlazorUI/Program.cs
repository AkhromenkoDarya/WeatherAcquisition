using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WeatherAcquisition.BlazorUI.Infrastructure.Extensions;
using WeatherAcquisition.Domain.Base;
using WeatherAcquisition.Interfaces.Base.Repositories;
using WeatherAcquisition.WebAPIClients.Repository;

namespace WeatherAcquisition.BlazorUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            IServiceCollection services = builder.Services;
            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            //services.AddHttpClient<IRepository<DataSourceInfo>, WebRepository<DataSourceInfo>>(
            //    (host, client) => client.BaseAddress = new Uri(
            //        host.GetRequiredService<IWebAssemblyHostEnvironment>().BaseAddress 
            //        + "api/SourceRepository")
            //    );

            services.AddApi<IRepository<DataSourceInfo>, WebRepository<DataSourceInfo>>(
                "api/SourceRepository");

            await builder.Build().RunAsync();
        }
    }
}
