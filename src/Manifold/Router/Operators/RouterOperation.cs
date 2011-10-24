using System;
using Manifold.Configuration;
using Manifold.Configuration.Pipeline;
using Manifold.Configuration.Pipeline.Operations;
using Manifold.DependencyInjection;

namespace Manifold.Router.Operators
{
    public class RouterOperation : IOperation
    {
        private readonly PipeDefinition _pipeDefinition;

        public RouterOperation(PipeDefinition pipeDefinition)
        {
            _pipeDefinition = pipeDefinition;

        }
        public Func<IPipelineContext, object, object> GetExecutor()
        {
            var compiler = new RouterCompiler(_pipeDefinition);

            var function = compiler.Compile();

            return function;
        }
    }
}
