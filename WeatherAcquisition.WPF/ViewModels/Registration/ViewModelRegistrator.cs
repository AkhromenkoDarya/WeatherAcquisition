using Microsoft.Extensions.DependencyInjection;

namespace WeatherAcquisition.WPF.ViewModels.Registration
{
    internal static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
           .AddSingleton<MainWindowViewModel>();
    }
}