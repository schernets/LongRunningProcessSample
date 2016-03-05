using System;
using LongRunningProcessSample.Events;
using LongRunningProcessSample.MessageBus;
using LongRunningProcessSample.States;
using MessageChannels = LongRunningProcessSample.MessageBus.MessageChannels;

namespace LongRunningProcessSample
{
    public class LongRunningProcess
    {
        public IMessageBus Bus { get; }
        private IState<LongRunningProcess> _currentState;

        public LongRunningProcess(Guid correlatinId, IMessageBus bus)
        {
            Bus = bus;
            CorrelationId = correlatinId;
            Bus.RegisterHandler<DownloadingFinished>(Handle, MessageChannels.SystemChannel);
            Bus.RegisterHandler<ProcessingFinished>(Handle, MessageChannels.SystemChannel);
        }

        private void Handle(DownloadingFinished message)
        {
            if (message.CorrelationId != CorrelationId) return;
            _currentState = new Processing(this);
            Request();
        }

        private void Handle(ProcessingFinished message)
        {
            if (message.CorrelationId != CorrelationId) return;
            _currentState = new Finished(this);
            Request();
        }
        
        public Guid CorrelationId { get; set; }

        public void Start()
        {
            _currentState = new Dowloading(this);
            Request();

        }

        protected void Request()
        {
            _currentState.Handle();
        }
    }
}