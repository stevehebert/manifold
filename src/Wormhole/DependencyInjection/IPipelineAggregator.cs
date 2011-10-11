using System;
using System.Collections.Generic;
using Wormhole.Configuration;
using Wormhole.PipeAndFilter;

namespace Wormhole.DependencyInjection
{
    public interface IPipelineAggregator
    {
        IDictionary<PipelineKey, Func<IResolveTypes, object, object>> Compile(IRegisterTypes typeRegistrar);
        PipelineConfigurator<TInput, TOutput> CreatePipeline<TType, TInput, TOutput>(TType name);
        PipelineConfigurator<TInput, TOutput> CreatePipeline<TInput, TOutput>();
        
    }
}