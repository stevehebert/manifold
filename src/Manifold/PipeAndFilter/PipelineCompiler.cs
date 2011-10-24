using System;
using System.Collections.Generic;
using Manifold.Configuration;
using Manifold.Configuration.Pipeline;
using Manifold.Configuration.Pipeline.Operations;
using Manifold.DependencyInjection;
using Manifold.Exceptions;

namespace Manifold.PipeAndFilter
{
    public class PipelineCompiler : IPipeCompiler
    {
        private readonly IPipeDefinition _pipeDefinition;
 
        public PipelineCompiler(IPipeDefinition pipeDefinition)
        {
            _pipeDefinition = pipeDefinition;
        }

        public Func<IPipelineContext, object, object> Compile()
        {
            if (!_pipeDefinition.Closed)
                throw new MismatchedClosingTypeDeclarationException();

            return Compile(new Queue<IOperation>(_pipeDefinition.Operations));
        }

        public Func<IPipelineContext, object, object> Compile(Queue<IOperation> operations)
        {
            var functionDefinition = operations.Dequeue().GetExecutor();
            while (operations.Count > 0)
            {
                var localFn = functionDefinition;
                var fn = operations.Dequeue().GetExecutor();

                functionDefinition = (a, b) => fn(a, localFn(a, b));
            }

            return functionDefinition;
        }
    }
}
