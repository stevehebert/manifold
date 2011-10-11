using Wormhole.Configuration;
using Wormhole.PipeAndFilter;

namespace Wormhole.Router
{
    public class PipelineRouterConfigurator<TInput, TOutputType, TOutput> : RouterConfigurator<TInput, TOutputType>
    {
        public PipelineRouterConfigurator(PipeDefinition pipeDefinition) : base(pipeDefinition.AddRoutingIntegrationStep())
        {}

        public new PipelineConfigurator<TOutputType, TOutput> Default<TType>() where TType : class, IPipelineTask<TInput, TOutputType>
        {
            PipeDefinition.AddInjectedOperation<TType, TInput, TOutputType>(true);
            return new PipelineConfigurator<TOutputType, TOutput>(PipeDefinition);
        }
    }
}