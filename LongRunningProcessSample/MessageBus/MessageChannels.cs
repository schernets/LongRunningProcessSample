namespace LongRunningProcessSample.MessageBus
{
	public static class MessageChannels
	{
	    public static IMessageChannel SystemChannel { get; } = new MessageChannel("SystemChannel");
	}
}