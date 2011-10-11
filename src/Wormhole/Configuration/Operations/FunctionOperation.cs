using System;
using Wormhole.DependencyInjection;

namespace Wormhole.Configuration.Operations
{
    public class FunctionOperation<TInput, TOutput> : IOperation
    {
        private readonly Func<TInput, TOutput> _function;

        public FunctionOperation(Func<TInput, TOutput> function )
        {
            _function = function;
        }
        public Func<IResolveTypes, object, object> GetExecutor()
        {
            return (injector, o) => _function( (TInput) o);
        }
    }
}