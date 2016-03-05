namespace LongRunningProcessSample
{
    public interface IState<out TContext>
    {
        TContext Context { get; }
        void Handle();
    }
}