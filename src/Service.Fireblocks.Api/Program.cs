using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MySettingsReader;
using Service.Fireblocks.Api.Settings;
using Microsoft.Extensions.Configuration;

namespace Service.Fireblocks.Api
{
    public class Program
    {
        public const string SettingsFileName = ".myjetwallet";

        public static SettingsModel Settings { get; private set; }

        public static EnvSettingsModel EnvSettings { get; private set; }

        public static ILoggerFactory LogFactory { get; private set; }

        public static Func<T> ReloadedSettings<T>(Func<SettingsModel, T> getter)
        {
            return () =>
            {
                var settings = SettingsReader.GetSettings<SettingsModel>(SettingsFileName);
                var value = getter.Invoke(settings);
                return value;
            };
        }

        public static void Main(string[] args)
        {
            Console.Title = "MyJetWallet Service.Fireblocks.Api";

            Settings = SettingsReader.GetSettings<SettingsModel>(SettingsFileName);
            EnvSettings = EnvReaderSettings();

            using var loggerFactory = LogConfigurator.ConfigureElk_v2("MyJetWallet", Settings.SeqServiceUrl, Settings.ElkLogs);

            var logger = loggerFactory.CreateLogger<Program>();

            LogFactory = loggerFactory;

            try
            {
                logger.LogInformation("Application is being started");

                CreateHostBuilder(loggerFactory, args).Build().Run();

                logger.LogInformation("Application has been stopped");
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Application has been terminated unexpectedly");
            }
        }

        public static IHostBuilder CreateHostBuilder(ILoggerFactory loggerFactory, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var httpPort = Environment.GetEnvironmentVariable("HTTP_PORT") ?? "8080";
                    var grpcPort = Environment.GetEnvironmentVariable("GRPC_PORT") ?? "80";

                    Console.WriteLine($"HTTP PORT: {httpPort}");
                    Console.WriteLine($"GRPC PORT: {grpcPort}");

                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, int.Parse(httpPort), o => o.Protocols = HttpProtocols.Http1);
                        options.Listen(IPAddress.Any, int.Parse(grpcPort), o => o.Protocols = HttpProtocols.Http2);
                    });

                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton(loggerFactory);
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                });

        private static EnvSettingsModel EnvReaderSettings()
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var result = configuration.Get<EnvSettingsModel>();

            if (string.IsNullOrEmpty(result.ENCRYPTION_KEY))
            {
                Console.WriteLine("Please set Env Variable EncryptionKey");
                throw new Exception("Please set Env Variable EncryptionKey");
            }

            return result;
        }
    }
}