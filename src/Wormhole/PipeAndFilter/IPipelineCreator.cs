using System;
using System.Linq;
using System.Text;

namespace Wormhole.PipeAndFilter
{
    public interface IPipelineCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>();
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name);


    }
    class PipelineCreator : IPipelineCreator
    {
        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
        {
            throw new NotImplementedException();
        }



        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name)
        {
            throw new NotImplementedException();
        }
    }
}
