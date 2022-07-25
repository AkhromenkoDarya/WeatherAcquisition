using System;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;
using WeatherAcquisition.WebAPIClients.Repository;
using WeatherAcquisition.WPF.Infrastructure.Extensions;
using WeatherAcquisition.WPF.Services.Registration;
using WeatherAcquisition.WPF.ViewModels.Registration;

namespace WeatherAcquisition.WPF
{
    public partial class App
    {
        public static Window ActiveWindow => Current.Windows.Cast<Window>().FirstOrDefault(w =>
            w.IsActive);

        public static Window FocusedWindow => Current.Windows.Cast<Window>().FirstOrDefault(w =>
            w.IsFocused);

        private static IHost _host;

        public static IHost Host => _host ??= Microsoft.Extensions.Hosting.Host
           .CreateDefaultBuilder(Environment.GetCommandLineArgs())
           .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json", true, 
               true))
           .ConfigureServices(ConfigureServices)
           .Build();

        public static IServiceProvider Services => Host.Services;

        private static void ConfigureServices(HostBuilderContext hostContext,
            IServiceCollection services)
        {
            services
                .AddViewModels()
                .AddServices();

            //services.AddHttpClient<IRepository<DataSource>, WebRepository<DataSource>>(
            //    client =>
            //    {
            //        // "/" в конце обязателен!
            //        client.BaseAddress = new Uri($"{hostContext.Configuration["WebApi"]}" +
            //                                     "api/DataSources/");
            //    });

            //services.AddHttpClient<IRepository<DataValue>, WebRepository<DataValue>>(
            //    client =>
            //    {
            //        client.BaseAddress = new Uri($"{hostContext.Configuration["WebApi"]}" +
            //                                     "api/DataValues/");
            //    });

            services.AddApi<IRepository<DataSource>, WebRepository<DataSource>>("api/DataSources/");
            services.AddApi<IRepository<DataValue>, WebRepository<DataValue>>("api/DataValues/");
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            IHost host = Host;
            base.OnStartup(e);
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            using IHost host = Host;
            await host.StopAsync();
        }
    }
}
