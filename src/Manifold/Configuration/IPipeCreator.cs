using Manifold.DependencyInjection;
using Manifold.PipeAndFilter;
using Manifold.Projector;

namespace Manifold.Configuration
{
    public interface IPipeCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>();
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name);

        ProjectorConfigurator<TInput, TOutput> RegisterProjector<TInput, TOutput>();
        ProjectorConfigurator<TInput, TOutput> RegisterProjector<TNameType, TInput, TOutput>(TNameType name);

        void Compile(IRegisterTypes typeRegistrar);
    }
}
