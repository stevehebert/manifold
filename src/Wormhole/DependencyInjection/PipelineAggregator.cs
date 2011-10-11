using System;
using System.Collections.Generic;
using System.Linq;
using Wormhole.PipeAndFilter;
using Wormhole.Pipeline;
using Wormhole.Pipeline.Configuration;
using PipelineCompiler = Wormhole.PipeAndFilter.PipelineCompiler;


namespace Wormhole.DependencyInjection
{


    public class PipelineAggregator
    {
        public IDictionary<PipelineKey, Func<IResolveTypes, object, object>> Compile()
        {
            return _aggregatePipelines.ToDictionary(value => value.Key, value => value.Value.Compile());
        }

        private readonly IDictionary<PipelineKey, IPipeCompiler> _aggregatePipelines =
            new Dictionary<PipelineKey, IPipeCompiler>();

        private readonly IList<Action<IRegisterTypes>> _registrationActions = new List<Action<IRegisterTypes>>();

        public PipeAndFilter.PipelineConfigurator<TInput,TOutput> CreatePipeline<TType, TInput, TOutput>(TType name) where TType : class, IPipelineTask<TInput, TOutput>
        {
            var definition = new PipelineDefinition(_registrationActions);

            _aggregatePipelines.Add(new PipelineKey
                                        {
                                            Input = typeof (TInput),
                                            Output = typeof (TOutput),
                                            Named = name
                                        }, new PipelineCompiler(definition));

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
            }, new PipelineCompiler(definition));

            return new PipeAndFilter.PipelineConfigurator<TInput, TOutput>(definition);
        }

    }

}
