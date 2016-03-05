using System;
using LongRunningProcessSample.MessageBus;

namespace LongRunningProcessSample.Events
{
    public class LrpFinished : Event
    {
        public LrpFinished(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}