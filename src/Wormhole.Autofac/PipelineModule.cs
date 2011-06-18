using Autofac;
using Wormhole.Autofac.Configuration;
using Wormhole.Pipeline;
using Wormhole.Pipeline.Configuration;

namespace Wormhole.Autofac
{
    public class PipelineModule : Module, IPipelineCreator
    {
        private readonly PipelineAggregator<TypeResolver> _pipelineAggregator = new PipelineAggregator<TypeResolver>();
        
        protected override void Load(ContainerBuilder builder)
        {
            _pipelineAggregator.PerformRegistration(new TypeRegistrar(builder));
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
            where TInput : class
            where TOutput : class
        {
            return _pipelineAggregator.RegisterPipeline<TInput, TOutput>();
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name)
            where TInput : class 
            where TOutput : class
        {
            return _pipelineAggregator.RegisterPipeline<TNameType, TInput, TOutput>(name);
        }
    }
}
