using System;
using System.Linq;
using System.Text;
using Wormhole.DependencyInjection;
using Wormhole.Pipeline;

namespace Wormhole.PipeAndFilter
{
    public interface IPipelineCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>();
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name) where TNameType : class, IPipelineTask<TInput, TOutput>;


    }


    internal class PipelineCreator : IPipelineCreator
    {
        private readonly PipelineAggregator _pipelineAggregator;

        public PipelineCreator(PipelineAggregator pipelineAggregator)
        {
            _pipelineAggregator = pipelineAggregator;
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
        {
            return _pipelineAggregator.CreatePipeline<TInput, TOutput>();

        }



        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name)
            where TNameType : class, IPipelineTask<TInput, TOutput>
        {
            return _pipelineAggregator.CreatePipeline<TNameType, TInput, TOutput>(name);
        }
    }
}
