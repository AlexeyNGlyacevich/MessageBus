using MessageBusExample.ViewModels;
using MessageBusExample.Views;
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
            // SerilogConfiguration
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();


            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(Log.Logger);

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
            var services = builder.Services;

            // UI, Views.
            services.AddSingleton<MainWindow>();

            // .. ViewModels.
            services.AddSingleton<MainWindowViewModel>();


            return builder;
        }
    }
}
