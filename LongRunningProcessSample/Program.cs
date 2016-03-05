using System;
using LongRunningProcessSample.Events;
using LongRunningProcessSample.MessageBus;

namespace LongRunningProcessSample
{
    internal class Program
    {
        private static void Main()
        {
            var bus = new MessageBus.MessageBus();
            bus.RegisterHandler<LrpFinished>(Handle, MessageChannels.SystemChannel);


            var correlationId = Guid.NewGuid();
            var lrp = new LongRunningProcess(correlationId, bus);
            lrp.Start();

            var correlationId1 = Guid.NewGuid();
            var lrp1 = new LongRunningProcess(correlationId1, bus);
            lrp1.Start();

            Console.ReadKey();
        }

        private static void Handle(LrpFinished lrpFinished)
        {
            Console.WriteLine($"FINISHED[{lrpFinished.CorrelationId}]");
        }
    }
}
