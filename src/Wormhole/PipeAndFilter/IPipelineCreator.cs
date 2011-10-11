using Wormhole.DependencyInjection;

namespace Wormhole.PipeAndFilter
{
    public interface IPipelineCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>();
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name);
        void Compile(IRegisterTypes typeRegistrar);
    }


    public class PipelineCreator : IPipelineCreator
    {
        private readonly IPipelineAggregator _pipelineAggregator;

        public PipelineCreator(IPipelineAggregator pipelineAggregator)
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

        public void Compile(IRegisterTypes typeRegistrar)
        {
            _pipelineAggregator.Compile(typeRegistrar);
        }
    }
}
