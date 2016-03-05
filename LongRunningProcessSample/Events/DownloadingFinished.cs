using System;
using LongRunningProcessSample.MessageBus;

namespace LongRunningProcessSample.Events
{
    public class DownloadingFinished : Event
    {
        public DownloadingFinished(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}