using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WeatherAcquisition.WPF.Infrastructure.Extensions
{
    internal static class ServiceExtensions
    {
        public static void AddApi<TInterface, TClient>(this IServiceCollection services, 
            string apiRoute) 
            where TInterface : class 
            where TClient : class, TInterface
        {
            services.AddHttpClient<TInterface, TClient>(client =>
            {
                string baseAddress = services
                    .BuildServiceProvider()
                    .GetRequiredService<HostBuilderContext>()
                    .Configuration["WebApi"];
                
                client.BaseAddress = new Uri($"{baseAddress}{apiRoute}");
            });
        }
    }
}
