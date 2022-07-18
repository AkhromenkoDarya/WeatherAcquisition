using Microsoft.Extensions.DependencyInjection;
using WeatherAcquisition.WPF.ViewModels;

namespace WeatherAcquisition.WPF.Locator
{
    internal class ServiceLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Services
            .GetRequiredService<MainWindowViewModel>();
    }
}
