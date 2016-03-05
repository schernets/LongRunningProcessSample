namespace LongRunningProcessSample.MessageBus
{
    public interface IEventPublisher
    {        
        void Publish<T>(T @event, IMessageChannel channel) where T : Event;
    }
}