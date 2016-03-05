namespace LongRunningProcessSample.MessageBus
{
    public interface ICommandSender
    {        
        void Send<T>(T command, IMessageChannel channel) where T : Command;
    }
}