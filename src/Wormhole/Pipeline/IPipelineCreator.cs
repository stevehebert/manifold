using Wormhole.Pipeline.Configuration;

namespace Wormhole.Pipeline
{
    public interface IPipelineCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>();

        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name);
    }
}
