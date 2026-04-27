using CommunityToolkit.Mvvm.Input;
using MessageBusExample.Models.MessageBusModels;
using MessageBusExample.Services;

namespace MessageBusExample.ViewModels
{
    public partial class MessageViewModel
    {
        private readonly ChannelMessageBus _messageBus;
        public MessageViewModel(ChannelMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        [RelayCommand]
        public void SendMessageToMainVM()
        {
            _messageBus.PublishTo<MainWindowViewModel>(new TextMessage("Привет главному окну от другого окна"));
        }

    }
}
