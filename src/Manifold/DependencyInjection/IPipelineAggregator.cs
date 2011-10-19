using System;
using System.Collections.Generic;
using Manifold.Configuration;
using Manifold.PipeAndFilter;
using Manifold.Projector;

namespace Manifold.DependencyInjection
{
    public interface IPipelineAggregator
    {
        IDictionary<PipelineKey, Func<IResolveTypes, object, object>> Compile(IRegisterTypes typeRegistrar);
        PipelineConfigurator<TInput, TOutput> CreatePipeline<TType, TInput, TOutput>(TType name);
        PipelineConfigurator<TInput, TOutput> CreatePipeline<TInput, TOutput>();

        ProjectorConfigurator<TInput, TOutput> CreateProjector<TInput, TOutput>();
    }
}