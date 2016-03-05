using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LongRunningProcessSample.MessageBus
{
    public class MessageBus : IMessageBus
    {       
        private static readonly object Locker = new object();

        private readonly 
            ConcurrentDictionary<
                KeyValuePair<Type, string>,
                List<KeyValuePair<Guid, Action<IMessage>>>> _routes =
                new ConcurrentDictionary<
                    KeyValuePair<Type, string>, 
                    List<KeyValuePair<Guid, Action<IMessage>>>>();

        public IReadOnlyDictionary<KeyValuePair<Type, string>, List<KeyValuePair<Guid, Action<IMessage>>>> Routes
            => new ReadOnlyDictionary<KeyValuePair<Type, string>, List<KeyValuePair<Guid, Action<IMessage>>>>(_routes);

        public Guid RegisterHandler<T>(Action<T> handle, IMessageChannel channel) where T : IMessage
        {
            var key = new KeyValuePair<Type, string>(typeof (T), channel.ChannelId);
            List<KeyValuePair<Guid, Action<IMessage>>> handlers;
            if (!_routes.TryGetValue(key, out handlers))
            {
                handlers = new List<KeyValuePair<Guid, Action<IMessage>>>();
                _routes.TryAdd(key, handlers);
            }
            lock (Locker)
            {
                var id = Guid.NewGuid();
                // TODO: I would prefer not to use the DelegateAdjuster magic. 
                handlers.Add(new KeyValuePair<Guid, Action<IMessage>>(id, DelegateAdjuster.CastArgument<IMessage, T>(x => handle(x))));
                return id;
            }
        }

        public void UnRegisterHandler(Guid token)
        {
            lock (Locker)
            {
                foreach (var messageTypeChannelHandlers in _routes.Values)
                {
                    var handler = messageTypeChannelHandlers.FirstOrDefault(x => x.Key == token);
                    if (handler.Key == token) messageTypeChannelHandlers.Remove(handler);
                }
            }
        }

        // This method is void and cannot be ran in isolation due to the usage of the ThreadPool. 
        // It cannot be 100% covered. It is hard to make conclusive assertions.
        public void Send<T>(T command, IMessageChannel channel) where T : Command
        {
            var messageChannelKey = new KeyValuePair<Type, string>(command.GetType(), channel.ChannelId);
            List<KeyValuePair<Guid, Action<IMessage>>> handlers;
            string errorMessage;
            if (!_routes.TryGetValue(messageChannelKey, out handlers))
            {
                errorMessage = "No handler registered";
                throw new InvalidOperationException(errorMessage);
            }

            if (handlers.Count != 1)
            {
                errorMessage = "Cannot send command to more than one handler";
                throw new InvalidOperationException(errorMessage);
            }
            ThreadPool.QueueUserWorkItem(x => { handlers[0].Value(command); });
        }

        /// <summary>
        /// Disseminates an event to all subscribers on a channel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <param name="channel"></param>

        // This method is void and cannot be ran in isolation due to the usage of the ThreadPool
        [ExcludeFromCodeCoverage]
        public void Publish<T>(T @event, IMessageChannel channel) where T : Event
        {
            var key = new KeyValuePair<Type, string>(@event.GetType(), channel.ChannelId);
            List<KeyValuePair<Guid, Action<IMessage>>> handlers;

            if (!_routes.TryGetValue(key, out handlers)) return;

            lock (Locker)
            {
                Parallel.ForEach(handlers.AsReadOnly(), route =>
                {
                    route.Value.Invoke(@event);
                });
            }

        }
    }
}
