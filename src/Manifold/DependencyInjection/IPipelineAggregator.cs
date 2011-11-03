using Manifold.PipeAndFilter;
using Manifold.Projector;

namespace Manifold.DependencyInjection
{
    public interface IPipelineAggregator
    {
        void Compile(IRegisterTypes typeRegistrar);
        PipelineConfigurator<TInput, TOutput> CreatePipeline<TType, TInput, TOutput>(TType name);
        PipelineConfigurator<TInput, TOutput> CreatePipeline<TInput, TOutput>();

        ProjectorConfigurator<TInput, TOutput> CreateProjector<TInput, TOutput>();
        ProjectorConfigurator<TInput, TOutput> CreateProjector<TNameType, TInput, TOutput>(TNameType name);
    }
}