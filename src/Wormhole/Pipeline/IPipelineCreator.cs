using Wormhole.Pipeline.Configuration;

namespace Wormhole.Pipeline
{
    public interface IPipelineCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
            where TInput : class
            where TOutput : class;

        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name)
            where TInput : class
            where TOutput : class;
    }
}
