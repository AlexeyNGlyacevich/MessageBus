using MessageBusExample.ViewModels;
using MessageBusExample.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MessageBusExample.Common
{
    public static class AppConfigurationExtension
    {

        public static HostApplicationBuilder ConfigureApplication(this HostApplicationBuilder builder)
        {
            // App Configuration
            builder.Configuration.AddJsonFile("appsettings.json", optional: false);

            // SerilogConfiguration
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();


            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();


            return builder;
        }


        public static HostApplicationBuilder ConfigureCore(this HostApplicationBuilder builder)
        {
            // Services, Logic, etc.

            var services = builder.Services;

            return builder;
        }

        public static HostApplicationBuilder ConfigureInfrastructure(this HostApplicationBuilder builder)
        {
            // DataBase, Repositories, etc.

            return builder;
        }

        public static HostApplicationBuilder ConfigureUI(this HostApplicationBuilder builder)
        {
            // UI, Views, ViewModels.

            var services = builder.Services;

            services.AddTransient<MainWindow>();


            services.AddTransient<MainWindowViewModel>();


            return builder;
        }
    }
}
