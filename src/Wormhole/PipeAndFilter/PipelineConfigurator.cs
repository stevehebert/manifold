using System;
using System.Collections.Generic;
using Wormhole.DependencyInjection;
using Wormhole.Pipeline;

namespace Wormhole.PipeAndFilter
{
    public class PipelineConfigurator<TInput, TOutput>
    {
        private readonly PipelineDefinition _pipelineDefinition;

        public PipelineConfigurator(PipelineDefinition pipelineDefiniton )
        {
            _pipelineDefinition = pipelineDefiniton;
        }

        public PipelineConfigurator<TOutputType, TOutput> Bind<TType, TOutputType>() where TType : class, IPipelineTask<TInput, TOutputType>
        {
            _pipelineDefinition.AddInjectedOperation<TType, TInput, TOutputType>();

            return new PipelineConfigurator<TOutputType, TOutput>(_pipelineDefinition);
        }

        public PipelineConfigurator<TInput, TOutput> Bind<TType>() where TType : class, IPipelineTask<TInput, TOutput>
        {
            _pipelineDefinition.AddInjectedOperation<TType, TInput, TOutput>();

            return new PipelineConfigurator<TInput, TOutput>(_pipelineDefinition);
        }
    }
}