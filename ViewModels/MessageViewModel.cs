using CommunityToolkit.Mvvm.Input;
using MessageBusExample.Abstractions.Windows;
using MessageBusExample.Models.LogModels;
using MessageBusExample.Models.MessageBusModels;
using MessageBusExample.Services;
using MessageBusExample.Views;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace MessageBusExample.ViewModels
{
    public partial class MessageViewModel
    {
        private readonly ChannelMessageBus _messageBus;

        private readonly IWindowsFactory _factory;

        private readonly Object _lock= new();
        public ObservableCollection<UILogEntry> Messages { get; } = []; 
        public MessageViewModel(ChannelMessageBus messageBus, IWindowsFactory factory)
        {
            _messageBus = messageBus;

            _factory = factory;

            BindingOperations.EnableCollectionSynchronization(Messages, _lock);

            _messageBus.Subscribe<TextMessage>(this, ReciveMessage);
        }

        [RelayCommand]
        public void SendMessageToMainVM()
        {
            _messageBus.PublishTo<MainWindowViewModel>(new TextMessage("Привет главному окну от другого окна", nameof(MessageViewModel)));
        }

        [RelayCommand]
        public void ShowAdditionalWindow()
        {
            var secondView = _factory.Create<AditionalMessageView>();

            secondView.Show();  
        }

        private async Task ReciveMessage(TextMessage msg)
        {
            Messages.Add(new UILogEntry
            {
                Timestamp = DateTime.UtcNow,
                Level = LogLevel.None.ToString(),
                Message = msg.Text,
                Source = msg.Source,
            });
        }
    }
}
