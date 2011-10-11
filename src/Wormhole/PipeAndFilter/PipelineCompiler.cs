using System;
using System.Collections.Generic;
using Wormhole.Configuration;
using Wormhole.DependencyInjection;
using Wormhole.Exceptions;

namespace Wormhole.PipeAndFilter
{
    public class PipelineCompiler : IPipeCompiler
    {
        private readonly IPipelineDefinition _pipelineDefinition;
 
        public PipelineCompiler(IPipelineDefinition pipelineDefinition)
        {
            _pipelineDefinition = pipelineDefinition;
        }

        public Func<IResolveTypes, object, object> Compile()
        {
            if (!_pipelineDefinition.Closed)
                throw new MismatchedClosingTypeDeclarationException();

            return Compile(new Queue<IOperation>(_pipelineDefinition.Operations));
        }

        public Func<IResolveTypes, object, object> Compile(Queue<IOperation> operations)
        {
            var functionDefinition = operations.Dequeue().GetClosure();
            while (operations.Count > 0)
            {
                var localFn = functionDefinition;
                var fn = operations.Dequeue().GetClosure();

                functionDefinition = (a, b) => fn(a, localFn(a, b));
            }

            return functionDefinition;
        }
    }
}
