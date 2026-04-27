using CommunityToolkit.Mvvm.Input;
using MessageBusExample.Models.MessageBusModels;
using MessageBusExample.Services;

namespace MessageBusExample.ViewModels
{
    public partial class AditionalViewModel
    {
        private readonly ChannelMessageBus _messageBus;

        public AditionalViewModel(ChannelMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        [RelayCommand]
        public void SendMessageToMainView()
        {
            _messageBus.PublishTo<MainWindowViewModel>(new TextMessage("Привет главному окну от дополнительного $$$ окна", nameof(AditionalViewModel)));
        }

        [RelayCommand]
        public void SendMessageToMessageView()
        {
            _messageBus.PublishTo<MessageViewModel>(new TextMessage("Привет информационному окну от дополнительного $$$ окна", nameof(AditionalViewModel)));
        }
    }
}
