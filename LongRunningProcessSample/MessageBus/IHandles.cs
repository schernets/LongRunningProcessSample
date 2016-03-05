namespace LongRunningProcessSample.MessageBus
{
    public interface IHandles<in T>
    {
        void Handle(T message);
    }
}