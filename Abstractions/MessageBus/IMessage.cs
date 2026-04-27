namespace MessageBusExample.Abstractions.MessageBus
{
    public interface IMessage
    {
    }

    public interface ITargetedMessage
    {
        Type TargetType { get; }
    }
}
