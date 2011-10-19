using System;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Pipeline.Operations
{
    public class RoutingFunctionOperation<TInput, TOutput> : IRoutedOperation
    {
        private readonly Func<TInput, bool> _canProcessFunction;
        private readonly Func<TInput, TOutput> _processFunction;
 
        public RoutingFunctionOperation(Func<TInput,bool> canProcessFunction, Func<TInput,TOutput> processFunction  )
        {
            _canProcessFunction = canProcessFunction;
            _processFunction = processFunction;

        }
        public Func<IResolveTypes, object, object> GetExecutor()
        {
            return (injector, o) => _processFunction((TInput) o);
        }

        public Func<IResolveTypes, object, bool> GetDecider()
        {
            return (injector, o) => _canProcessFunction((TInput) o);
        }
    }
}
