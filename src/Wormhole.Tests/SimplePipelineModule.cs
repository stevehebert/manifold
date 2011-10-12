using System;
using Manifold.Autofac;
using Manifold.Configuration;

namespace Manifold.Tests
{
    public class SimplePipelineModule : PipelineModule
    {
        private readonly Action<IPipeCreator> _pipelineCreator;

        public SimplePipelineModule(Action<IPipeCreator> pipelineCreator)
        {
            _pipelineCreator = pipelineCreator;
        }

        public override void RegisterPipelines(IPipeCreator pipeCreator)
        {
            _pipelineCreator(pipeCreator);
        }
    }
}
