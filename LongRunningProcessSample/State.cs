namespace LongRunningProcessSample
{
    public abstract class State<TContext> : IState<TContext>
    {
        protected State(TContext context)
        {
            Context = context;
        }
        public TContext Context { get; }
        public abstract void Handle();
    }
}