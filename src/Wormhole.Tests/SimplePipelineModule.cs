using System;
using Wormhole.Autofac;
using Wormhole.PipeAndFilter;

namespace Wormhole.Tests
{
    public class SimplePipelineModule : PipelineModule
    {
        private Action<IPipelineCreator> _pipelineCreator;

        public SimplePipelineModule(Action<IPipelineCreator> pipelineCreator)
        {
            _pipelineCreator = pipelineCreator;
        }

        public override void RegisterPipelines(IPipelineCreator pipelineCreator)
        {
            _pipelineCreator(pipelineCreator);
        }
    }
}
