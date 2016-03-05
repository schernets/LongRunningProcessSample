using System;
using System.Threading;
using System.Threading.Tasks;
using LongRunningProcessSample.Events;
using LongRunningProcessSample.MessageBus;

namespace LongRunningProcessSample.States
{
    public sealed class Dowloading : State<LongRunningProcess>
    {
        public Dowloading(LongRunningProcess context) : base(context)
        {
        }

        public override void Handle()
        {
            Console.WriteLine("Application thread ID: {0}", Thread.CurrentThread.ManagedThreadId);
            var t = Task.Run(() => {
                Console.WriteLine("Downloading file on task thread ID: {0}",Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Downloading for 5 seconds...");
                Thread.Sleep(5000);
                Context.Bus.Publish(new DownloadingFinished(Context.CorrelationId), MessageChannels.SystemChannel);
            });
        }
    }
}