using MessageBusExample.Abstractions.MessageBus;

namespace MessageBusExample.Models.MessageBusModels
{
    public sealed record TextMessage(string Text) : IMessage
    {
    }
}
