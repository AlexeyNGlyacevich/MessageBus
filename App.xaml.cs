using MessageBusExample.Common;
using MessageBusExample.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Windows;

namespace MessageBusExample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? _hosting;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            _hosting = Host.CreateApplicationBuilder()
                .ConfigureCore()
                .ConfigureInfrastructure()
                .ConfigureUI()
                .ConfigureApplication()
                .Build();

            // Логирование ошибок UI
            SetupGlobalExceptionHandling();

            await _hosting.StartAsync();


            var mainWindow = _hosting.Services.GetRequiredService<MainWindow>();

            Current.MainWindow = mainWindow;

            mainWindow.Show();
        }


        protected override async void OnExit(ExitEventArgs e)
        {
            if (_hosting is not null)
            {
                await _hosting.StopAsync();
                _hosting.Dispose();
            }

            // Важно! для Serilog
            Log.CloseAndFlush();

            base.OnExit(e);
        }

        private void SetupGlobalExceptionHandling()
        {
            DispatcherUnhandledException += (_, ex) =>
            {
                Log.Error(ex.Exception, "UI thread exception");
                ex.Handled = true; // или false если хочешь crash
            };

            AppDomain.CurrentDomain.UnhandledException += (_, ex) =>
            {
                Log.Error(ex.ExceptionObject as Exception, "Non-UI exception");
            };

            TaskScheduler.UnobservedTaskException += (_, ex) =>
            {
                Log.Error(ex.Exception, "Task exception");
                ex.SetObserved();
            };
        }
    }

}
