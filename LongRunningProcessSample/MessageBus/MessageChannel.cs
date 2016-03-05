namespace LongRunningProcessSample.MessageBus
{
	public sealed class MessageChannel : IMessageChannel
    {
        private readonly int _hashCode;
        public MessageChannel(string channelId)
        {
            ChannelId = channelId;
            _hashCode = new { ChannelId }.GetHashCode();
        }

        public string ChannelId { get; }

	    public override int GetHashCode()
		{
		    return _hashCode;
		}

		public override string ToString()
		{
			return ChannelId;
		}
	}
}