using System;
using System.Collections.Generic;

namespace LongRunningProcessSample.MessageBus
{
    public interface IMessageBus : ICommandSender, IEventPublisher
    {        
        Guid RegisterHandler<T>(Action<T> handle, IMessageChannel channel) where T : IMessage;
        void UnRegisterHandler(Guid token);
        IReadOnlyDictionary<KeyValuePair<Type, string>, List<KeyValuePair<Guid, Action<IMessage>>>> Routes { get; }
    }
}