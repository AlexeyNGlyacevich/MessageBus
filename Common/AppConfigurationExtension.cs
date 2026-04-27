using MessageBusExample.Abstractions.Windows;
using MessageBusExample.Services;
using MessageBusExample.ViewModels;
using MessageBusExample.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MessageBusExample.Common
{
    public static class AppConfigurationExtension
    {
        public static HostApplicationBuilder ConfigureApplication(this HostApplicationBuilder builder)
        {
            // SerilogConfiguration
            builder.Services.AddSerilog((services, lc) =>
            {
                lc.ReadFrom.Configuration(builder.Configuration)
                  .ReadFrom.Services(services)
                  .Enrich.FromLogContext()
                  .WriteTo.Sink(services.GetRequiredService<UILogSink>());
            });

            return builder;
        }


        public static HostApplicationBuilder ConfigureCore(this HostApplicationBuilder builder)
        {
            // Services, Logic, etc.

            var services = builder.Services;

            services.AddSingleton<UILogStore>();
            services.AddSingleton<UILogSink>();

            services.AddSingleton<ChannelMessageBus>();
            services.AddSingleton<IWindowsFactory, WindowsFactory>();

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
            services.AddTransient<DialogMessageView>();

            // .. ViewModels.
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<MessageViewModel>();


            return builder;
        }
    }
}
