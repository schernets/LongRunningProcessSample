using System;
using LongRunningProcessSample.MessageBus;

namespace LongRunningProcessSample.Events
{
    public class ProcessingFinished : Event
    {
        public ProcessingFinished(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}