using Microsoft.Extensions.DependencyInjection;
using WeatherAcquisition.WPF.Services.Interfaces;

namespace WeatherAcquisition.WPF.Services.Registration
{
    static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
           .AddTransient<IDataService, DataService>()
           .AddTransient<IUserDialog, UserDialog>();
    }
}
