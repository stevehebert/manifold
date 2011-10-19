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

        void Compile(IRegisterTypes typeRegistrar);
    }
}
