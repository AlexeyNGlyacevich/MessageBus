using CommunityToolkit.Mvvm.Input;
using MessageBusExample.Abstractions.Windows;
using MessageBusExample.Models.LogModels;
using MessageBusExample.Models.MessageBusModels;
using MessageBusExample.Services;
using MessageBusExample.Views;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace MessageBusExample.ViewModels
{
    public partial class MainWindowViewModel
    {
        private readonly ILogger<MainWindowViewModel> _logger;

        private readonly ChannelMessageBus _messageBus;

        private readonly IWindowsFactory _factory;

        private readonly UILogStore _store;

        
        public ObservableCollection<UILogEntry> Logs => _store.Logs;

        public MainWindowViewModel(ILogger<MainWindowViewModel> logger, ChannelMessageBus bus, UILogStore store, IWindowsFactory factory)
        {
            _logger = logger;

            _messageBus = bus;

            _factory = factory;

            _store = store;

            _logger.LogInformation("Main Window Started");

            _messageBus.Subscribe<TextMessage>(this, ReciveMessageAsync);
        }

        [RelayCommand]
        private void SendLogMessage()
        {
            _logger.LogInformation($"Информационное сообщение + {Logs.Count}");
        }

        [RelayCommand]
        public void ActivateDependentView()
        {
            var view = _factory.Create<DialogMessageView>();

            view.Show();
        }

        private async Task ReciveMessageAsync(TextMessage msg)
        {
            Logs.Add(new UILogEntry   
            { 
                    Timestamp = DateTime.UtcNow,
                    Level = LogLevel.Debug.ToString(),
                    Message = msg.Text,
                    Source = "VM"
            });
        }
    }
}
