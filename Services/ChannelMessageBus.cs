using MessageBusExample.Abstractions.MessageBus;
using MessageBusExample.Models.MessageBusModels;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace MessageBusExample.Services
{
    public class ChannelMessageBus
    {
        private readonly Channel<RoutedMessage> _channel;

        private readonly ConcurrentDictionary<Type, List<Subscription>> _subscriptions = new();

        private readonly ConcurrentDictionary<Type, SemaphoreSlim> _locks = new();

        public ChannelMessageBus()
        {
            _channel = Channel.CreateUnbounded<RoutedMessage>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
        }

        // =====================
        // LOCK PER MESSAGE TYPE
        // =====================
        private SemaphoreSlim GetLock(Type messageType)
            => _locks.GetOrAdd(messageType, _ => new SemaphoreSlim(1, 1));

        // =====================
        // PUBLISH (broadcast)
        // =====================
        public void Publish(IMessage message)
        {
            _channel.Writer.TryWrite(RoutedMessage.Broadcast(message));
        }

        // =====================
        // PUBLISH (targeted)
        // =====================
        public void PublishTo<TReceiver>(IMessage message) where TReceiver : class
        {
            _channel.Writer.TryWrite(RoutedMessage.To<TReceiver>(message));
        }

        // =====================
        // SUBSCRIBE
        // =====================
        public void Subscribe<TMessage>(
            object subscriber,
            Func<TMessage, Task> handler)
            where TMessage : IMessage
        {
            var sub = new Subscription(
                typeof(TMessage),
                subscriber,
                msg => handler((TMessage)msg));

            var list = _subscriptions.GetOrAdd(typeof(TMessage), _ => new List<Subscription>());

            lock (list)
            {
                list.Add(sub);
            }
        }

        // =====================
        // START LOOP
        // =====================
        public async Task StartAsync(CancellationToken ct)
        {
            await foreach (var routed in _channel.Reader.ReadAllAsync(ct))
            {
                await Dispatch(routed);
            }
        }

        // =====================
        // DISPATCHER
        // =====================
        private async Task Dispatch(RoutedMessage routed)
        {
            var messageType = routed.Message.GetType();

            if (!_subscriptions.TryGetValue(messageType, out var list))
                return;

            var semaphore = GetLock(messageType);

            List<Subscription> snapshot;
            List<Subscription> alive = new();
            List<Task> tasks = new();

            await semaphore.WaitAsync();
            try
            {
                // 1. snapshot
                lock (list)
                {
                    snapshot = list.ToList();
                }

                // 2. фильтрация
                foreach (var sub in snapshot)
                {
                    if (!sub.TryGetTarget(out var target))
                        continue;

                    alive.Add(sub);

                    bool matchesTarget =
                        routed.TargetType == null ||
                        routed.TargetType.IsAssignableFrom(sub.SubscriberType);

                    if (matchesTarget)
                    {
                        tasks.Add(SafeInvoke(sub, routed.Message));
                    }
                }

                // 3. cleanup — только мёртвых убираем
                lock (list)
                {
                    list.Clear();
                    list.AddRange(alive);
                }
            }
            finally
            {
                semaphore.Release();
            }

            // 4. выполнение вне lock
            if (tasks.Count > 0)
            {
                try
                {
                    await Task.WhenAll(tasks);
                }
                catch
                {
                    // логирование по желанию
                }
            }
        }

        private async Task SafeInvoke(Subscription sub, IMessage message)
        {
            try
            {
                await sub.Handler(message);
            }
            catch
            {
                // сюда можно подключить ILogger
            }
        }
    }
}
