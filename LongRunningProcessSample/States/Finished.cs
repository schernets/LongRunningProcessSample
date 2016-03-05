using LongRunningProcessSample.Events;
using LongRunningProcessSample.MessageBus;

namespace LongRunningProcessSample.States
{
    internal class Finished : IState<LongRunningProcess>
    {
        public Finished(LongRunningProcess context)
        {
            Context = context;
        }

        public LongRunningProcess Context { get; }
        public void Handle()
        {
            Context.Bus.Publish(new LrpFinished(Context.CorrelationId), MessageChannels.SystemChannel);
        }
    }
}