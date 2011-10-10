using System;

namespace Wormhole.DependencyInjection
{
    public class FunctionOperation<TInput, TOutput> : IOperation
    {
        private readonly Func<TInput, TOutput> _function;

        public FunctionOperation(Func<TInput, TOutput> function )
        {
            _function = function;
        }
        public Func<IResolveTypes, object, object> GetClosure()
        {
            return (injector, o) => _function( (TInput) o);
        }
    }
}