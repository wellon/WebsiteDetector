using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Microsoft.Extensions.Options;
using WebsiteDetector.Configuration;
using WebsiteDetector.Services;
using WebsiteDetector.Infrastructure;
using System.Net.Http;

namespace WebsiteDetector
{
    class Program
    {
        private static IServiceProvider serviceProvider;
        private static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            BuildConfiguration();
            RegisterServices();

            Start();

            DisposeServices();
        }

        private static void Start()
        {
            var messagesService = serviceProvider.GetService<IMessagesService>();

            messagesService.SendWelcomeMessage();

            while (true)
            {
                var command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        serviceProvider.GetService<IAvailabilityDetectorService>().Start();
                        break;
                    case "2":
                        serviceProvider.GetService<IAvailabilityDetectorService>().Stop();
                        break;
                }
            }
        }

        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
        }


        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<WebsitesConfig>(configuration);
            services.AddTransient<WebsitesConfig>(ctx => ctx.GetRequiredService<IOptions<WebsitesConfig>>().Value);
            services.AddTransient(ctx => new HttpClient());
            services.AddTransient<IMessagesService, ConsoleMessagesService>();
            services.AddSingleton<IAvailabilityDetectorService, AvailabilityDetectorService>();
            serviceProvider = services.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (serviceProvider == null)
            {
                return;
            }
            if (serviceProvider is IDisposable)
            {
                ((IDisposable)serviceProvider).Dispose();
            }
        }
    }
}
