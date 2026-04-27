using MessageBusExample.Abstractions.MessageBus;

namespace MessageBusExample.Models.MessageBusModels
{
    public sealed class RoutedMessage
    {
        public IMessage Message { get; }
        public Type? TargetType { get; }

        public RoutedMessage(IMessage message, Type? targetType = null)
        {
            Message = message;
            TargetType = targetType;
        }

        public static RoutedMessage Broadcast(IMessage message) 
            => new(message);

        public static RoutedMessage To<TReceiver>(IMessage message) where TReceiver : class
            => new(message, typeof(TReceiver));
    }
}
