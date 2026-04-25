using CommunityToolkit.Mvvm.Input;
using MessageBusExample.Models.LogModels;
using MessageBusExample.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace MessageBusExample.ViewModels
{
    public partial class MainWindowViewModel
    {
        private readonly ILogger<MainWindowViewModel> _logger;
        private readonly ChannelMessageBus _messageBus;
        
        public ObservableCollection<UILogEntry> Logs { get; }

        public MainWindowViewModel(ILogger<MainWindowViewModel> logger, ChannelMessageBus bus, UILogStore store)
        {
            _logger = logger;
            _messageBus = bus;

            Logs = store.Logs;

            _logger.LogInformation("Main Window Started");
        }

        [RelayCommand]
        private void SendLogMessage()
        {
            _logger.LogInformation($"Информационное сообщение + {Logs.Count}");
        }
    }
}
