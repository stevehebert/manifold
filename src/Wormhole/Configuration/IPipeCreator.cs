using Manifold.DependencyInjection;
using Manifold.PipeAndFilter;

namespace Manifold.Configuration
{
    public interface IPipeCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>();
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name);

        void Compile(IRegisterTypes typeRegistrar);
    }
}
