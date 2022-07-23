using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherAcquisition.BlazorUI.Infrastructure.Extensions
{
    internal static class ServiceExtensions
    {
        public static IHttpClientBuilder AddApi<TInterface, TClient>(
            this IServiceCollection services, string apiAddress)
            where TInterface : class
            where TClient : class, TInterface => 
            services
                .AddHttpClient<TInterface, TClient>(
                    (host, client) => client.BaseAddress = new(
                        $"{host.GetRequiredService<IWebAssemblyHostEnvironment>().BaseAddress}" +
                        $"{apiAddress}")
                    );
    }
}
