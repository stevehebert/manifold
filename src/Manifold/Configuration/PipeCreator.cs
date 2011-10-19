using Manifold.DependencyInjection;
using Manifold.PipeAndFilter;
using Manifold.Projector;

namespace Manifold.Configuration
{
    public class PipeCreator : IPipeCreator
    {
        private readonly IPipelineAggregator _pipelineAggregator;

        public PipeCreator(IPipelineAggregator pipelineAggregator)
        {
            _pipelineAggregator = pipelineAggregator;
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
        {
            return _pipelineAggregator.CreatePipeline<TInput, TOutput>();
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name)
        {
            return _pipelineAggregator.CreatePipeline<TNameType, TInput, TOutput>(name);
        }

        public ProjectorConfigurator<TInput, TOutput> RegisterProjector<TInput, TOutput>()
        {
            return _pipelineAggregator.CreateProjector<TInput, TOutput>();
        }

        public void Compile(IRegisterTypes typeRegistrar)
        {
            _pipelineAggregator.Compile(typeRegistrar);
        }
    }
}