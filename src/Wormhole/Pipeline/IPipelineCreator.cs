using System;
using Wormhole.DependencyInjection;
using Wormhole.Pipeline.Configuration;

namespace Wormhole.Pipeline
{
    public interface IPipelineCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>();

        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name);
    }

    public class PipelineCreator<TResolver> : IPipelineCreator where TResolver : IResolveTypes
    {
        private readonly PipelineAggregator<TResolver> _pipelineAggregator = new PipelineAggregator<TResolver>();

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
        {
            return _pipelineAggregator.RegisterPipeline<TInput, TOutput>();
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name)
        {
            return _pipelineAggregator.RegisterPipeline<TNameType, TInput, TOutput>(name);
        }
        
        public void PerformRegistration(IRegisterTypes registrar)
        {
            _pipelineAggregator.PerformRegistration(registrar);
        }
    }
}
