using Microsoft.Extensions.DependencyInjection;

namespace WeatherAcquisition.WPF.ViewModels.Locator
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindow => App.Services.GetRequiredService<MainWindowViewModel>();
    }
}
