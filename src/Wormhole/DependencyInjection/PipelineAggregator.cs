using System;
using System.Collections.Generic;
using Wormhole.PipeAndFilter;
using Wormhole.Pipeline;
using Wormhole.Pipeline.Configuration;


namespace Wormhole.DependencyInjection
{
    public class PipelineAggregator
    {
        private readonly IDictionary<PipelineKey, PipelineDefinition> _aggregatePipelines =
            new Dictionary<PipelineKey, PipelineDefinition>();

        private readonly IList<Action<IRegisterTypes>> _registrationActions = new List<Action<IRegisterTypes>>();

        public PipeAndFilter.PipelineConfigurator<TInput,TOutput> CreatePipeline<TType, TInput, TOutput>(TType name) where TType : class, IPipelineTask<TInput, TOutput>
        {
            var definition = new PipelineDefinition(_registrationActions);

            _aggregatePipelines.Add(new PipelineKey
                                        {
                                            Input = typeof (TInput),
                                            Output = typeof (TOutput),
                                            Named = name
                                        }, definition);

            return new PipeAndFilter.PipelineConfigurator<TInput, TOutput>(definition);
        }

        public PipeAndFilter.PipelineConfigurator<TInput, TOutput> CreatePipeline<TInput, TOutput>() 
        {
            var definition = new PipelineDefinition(_registrationActions);

            _aggregatePipelines.Add(new PipelineKey
            {
                Input = typeof(TInput),
                Output = typeof(TOutput),
                Named = new DefaultPipeline<TInput, TOutput>()
            }, definition);

            return new PipeAndFilter.PipelineConfigurator<TInput, TOutput>(definition);
        }

    }

}
