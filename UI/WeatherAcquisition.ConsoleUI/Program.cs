using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;
using WeatherAcquisition.WebAPIClients.Repository;

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
            services.AddHttpClient<IRepository<DataSource>, WebRepository<DataSource>>(
                client =>
                {
                    // "/" в конце обязателен!
                    client.BaseAddress = new Uri($"{hostContext.Configuration["WebApi"]}" +
                                                 "api/DataSources/");
                });

            services.AddHttpClient<IRepository<DataValue>, WebRepository<DataValue>>(
                client =>
                {
                    client.BaseAddress = new Uri($"{hostContext.Configuration["WebApi"]}" +
                                                 "api/DataValues/");
                });
        }

        static async Task Main(string[] args)
        {
            using IHost host = Hosting;
            await host.StartAsync();

            var dataSources = Services.GetRequiredService<IRepository<DataSource>>();

            int count = await dataSources.GetCount();

            IEnumerable<DataSource> sources = await dataSources.GetAll();

            foreach (DataSource src in sources)
            {
                Console.WriteLine($"{src.Id}: {src.Name} - {src.Description}");
            }

            DataSource source = await dataSources.GetById(555);

            sources = await dataSources.Get(5, 4);

            IPage<DataSource> page = await dataSources.GetPage(4, 354);

            DataSource newSource = await dataSources.Add(
                new DataSource
                {
                    Name = "I am the new data source!",
                    Description = "It's my amazing description!!!"
                });

            DataSource updatedSource = await dataSources.Update(
                new DataSource
                {
                    Id = 10,
                    Name = $"Name at {DateTime.Now:HH-mm-ss}",
                    Description = $"Description at {DateTime.Now:HH-mm-ss}"
                });

            //DataSource[] firstItems = (await dataSources.Get(0, 2)).ToArray();

            DataSource[] firstItems = (await dataSources.GetFirsts(-3)).ToArray();

            DataSource firstDeletedItem = await dataSources.DeleteById(firstItems[0].Id);
            DataSource secondDeletedItem = await dataSources.Delete(firstItems[1]);

            DataSource[] lastItems = (await dataSources.GetLasts(2)).ToArray();

            firstDeletedItem = await dataSources.DeleteById(lastItems[0].Id);
            secondDeletedItem = await dataSources.Delete(lastItems[1]);

            Console.WriteLine("Done!");
            Console.ReadLine();

            await host.StopAsync();
        }
    }
}
