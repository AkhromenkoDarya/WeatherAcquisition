using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WeatherAcquisition.ConsoleUI
{
    internal class Program
    {
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
            
        }

        static async Task Main(string[] args)
        {
            using IHost host = Hosting;
            await host.StartAsync();

            Console.WriteLine("Done!");
            Console.ReadLine();

            await host.StopAsync();
        }
    }
}
