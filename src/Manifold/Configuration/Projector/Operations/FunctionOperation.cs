using System;
using System.Collections.Generic;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Projector.Operations
{
    public class FunctionOperation<TInput, TOutput> : IProjectorOperation<TInput, TOutput>
    {
        private readonly Func<TInput, IEnumerable<TOutput>> _functionOperation;

        public FunctionOperation(Func<TInput, IEnumerable<TOutput>> functionOperation)
        {
            _functionOperation = functionOperation;
        }
        public Func<IPipelineContext, TInput, IEnumerable<TOutput>> GetExecutor()
        {
            return (injector, o) => _functionOperation(o);
        }
    }
}
