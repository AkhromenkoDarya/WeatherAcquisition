using System;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherAcquisition.WPF.ViewModels;

namespace WeatherAcquisition.WPF
{
    public partial class App
    {
        public static Window ActiveWindow => Current.Windows.Cast<Window>()
            .FirstOrDefault(w => w.IsActive);

        public static Window FocusedWindow => Current.Windows.Cast<Window>()
            .FirstOrDefault(w => w.IsFocused);

        public static Window CurrentWindow => ActiveWindow ?? FocusedWindow;

        private static IHost _hosting;

        public static IHost Hosting => _hosting ??= CreateHostBuilder(Environment
            .GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Hosting.Services;

        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices)
        ;

        private static void ConfigureServices(HostBuilderContext hostContext, 
            IServiceCollection services)
        {
            services.AddScoped<MainWindowViewModel>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            IHost host = Hosting;
            base.OnStartup(e);
            await host.StartAsync().ConfigureAwait(false);
            //Services.GetRequiredService<MainWindow>().Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using IHost host = Hosting;
            base.OnExit(e);
            await host.StopAsync().ConfigureAwait(false);
        }
    }
}
