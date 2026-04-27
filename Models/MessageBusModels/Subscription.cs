using MessageBusExample.Abstractions.MessageBus;

namespace MessageBusExample.Models.MessageBusModels
{
    public sealed class Subscription
    {
        public Type MessageType { get; }
        public Type SubscriberType { get; }
        public WeakReference Target { get; }
        public Func<IMessage, Task> Handler { get; }

        public Subscription(Type messageType, object subscriber, Func<IMessage, Task> handler)
        {
            MessageType = messageType;
            SubscriberType = subscriber.GetType();
            Target = new WeakReference(subscriber);
            Handler = handler;
        }

        public bool TryGetTarget(out object? target)
        {
            target = Target.Target;
            return target != null;
        }
    }
}
