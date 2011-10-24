using System;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Pipeline.Operations
{
    public class FunctionOperation<TInput, TOutput> : IOperation
    {
        private readonly Func<TInput, TOutput> _function;

        public FunctionOperation(Func<TInput, TOutput> function )
        {
            _function = function;
        }
        public Func<IPipelineContext, object, object> GetExecutor()
        {
            return (injector, o) => _function( (TInput) o);
        }
    }
}