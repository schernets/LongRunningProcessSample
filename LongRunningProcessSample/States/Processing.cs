using System;
using System.Threading;
using System.Threading.Tasks;
using LongRunningProcessSample.Events;
using LongRunningProcessSample.MessageBus;

namespace LongRunningProcessSample.States
{
    public sealed class Processing : State<LongRunningProcess>
    {
        public Processing(LongRunningProcess context) : base(context)
        {
        }

        public override void Handle()
        {
            Console.WriteLine("Application thread ID: {0}", Thread.CurrentThread.ManagedThreadId);
            var t = Task.Run(() => {
                Console.WriteLine("Processing file on task thread ID: {0}", Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Processing for 10 seconds...");
                Thread.Sleep(10000);
                Context.Bus.Publish(new ProcessingFinished(Context.CorrelationId), MessageChannels.SystemChannel);
            });

        }
    }
}