using System;
using System.Linq.Expressions;

namespace LongRunningProcessSample.MessageBus
{

    //FROM http://blogs.msdn.com/b/davidebb/archive/2010/01/18/use-c-4-0-dynamic-to-drastically-simplify-your-private-reflection-code.aspx
    //doesnt count to line counts :)


    public static class DelegateAdjuster
    {
        public static Action<TBaseT> CastArgument<TBaseT, TDerivedT>(Expression<Action<TDerivedT>> source) where TDerivedT : TBaseT
        {
            if (typeof(TDerivedT) == typeof(TBaseT))
            {
                return (Action<TBaseT>)((Delegate)source.Compile());

            }
            var sourceParameter = Expression.Parameter(typeof(TBaseT), "source");
            var result = Expression.Lambda<Action<TBaseT>>(
                Expression.Invoke(
                    source,
                    Expression.Convert(sourceParameter, typeof(TDerivedT))),
                sourceParameter);
            return result.Compile();
        }
    }
}

