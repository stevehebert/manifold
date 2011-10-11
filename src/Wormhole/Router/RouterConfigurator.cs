using Wormhole.Configuration;
using Wormhole.PipeAndFilter;

namespace Wormhole.Router
{
    public class PipelineRouterConfigurator<TInput, TOutputType, TOutput> : RouterConfigurator<TInput, TOutputType>
    {
        public PipelineRouterConfigurator(PipeDefinition pipeDefinition) : base (pipeDefinition)
        {}

        public new PipelineConfigurator<TOutputType, TOutput> Default<TType>() where TType : class, IPipelineTask<TInput, TOutputType>
        {
            PipeDefinition.AddInjectedOperation<TType, TInput, TOutputType>(true);
            return new PipelineConfigurator<TOutputType, TOutput>(PipeDefinition);
        }
    }

    public class RouterConfigurator<TInput, TOutput>
    {
        protected readonly PipeDefinition PipeDefinition;

        public RouterConfigurator(PipeDefinition pipeDefinition)
        {
            PipeDefinition = pipeDefinition;
        }

        public RouterConfigurator<TInput, TOutput> BindConditional<TType>() where TType : class, IRoutedPipelineTask<TInput, TOutput>
        {
            PipeDefinition.AddInjectedRouteOperation<TType, TInput, TOutput>();
            return this;
        }

        public void Default<TType>() where TType : class, IPipelineTask<TInput, TOutput>
        {
            PipeDefinition.AddInjectedOperation<TType, TInput, TOutput>(true);
        }
    }
}
