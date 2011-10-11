using System;
using System.Collections.Generic;
using System.Linq;
using Wormhole.Configuration;
using Wormhole.PipeAndFilter;
using PipelineCompiler = Wormhole.PipeAndFilter.PipelineCompiler;


namespace Wormhole.DependencyInjection
{
    public interface IPipelineAggregator
    {
        IDictionary<PipelineKey, Func<IResolveTypes, object, object>> Compile(IRegisterTypes typeRegistrar);
        PipelineConfigurator<TInput, TOutput> CreatePipeline<TType, TInput, TOutput>(TType name);
        PipelineConfigurator<TInput, TOutput> CreatePipeline<TInput, TOutput>();
    }

    public class PipelineAggregator<TResolver> : IPipelineAggregator where TResolver : IResolveTypes
    {
        public PipelineAggregator()
        {
            _registrationActions.Add(a => a.RegisterType<TResolver, IResolveTypes>());
        }
        public IDictionary<PipelineKey, Func<IResolveTypes, object, object>> Compile(IRegisterTypes typeRegistrar)
        {
            var dictionary = _aggregatePipelines.ToDictionary(value => value.Key, value => value.Value.Compile()) as IDictionary<PipelineKey, Func<IResolveTypes, object, object>>;
            typeRegistrar.RegisterInstance(dictionary);

            foreach (var item in _registrationActions)
                item(typeRegistrar);

            return dictionary;
        }

        private readonly IDictionary<PipelineKey, IPipeCompiler> _aggregatePipelines =
            new Dictionary<PipelineKey, IPipeCompiler>();

        private readonly IList<Action<IRegisterTypes>> _registrationActions = new List<Action<IRegisterTypes>>();



        public PipelineConfigurator<TInput,TOutput> CreatePipeline<TType, TInput, TOutput>(TType name) 
        {
            var definition = new PipeDefinition(_registrationActions);

            _aggregatePipelines.Add(new PipelineKey
                                        {
                                            Input = typeof (TInput),
                                            Output = typeof (TOutput),
                                            Named = name
                                        }, new PipelineCompiler(definition));

            _registrationActions.Add(a => a.Register<Pipe<TType, TInput, TOutput>>(ctx =>
                                                                                        {
                                                                                            var op =
                                                                                                new NamedResolutionOperation
                                                                                                    <TInput,
                                                                                                        TOutput, TType>(name);
                                                                                            return
                                                                                                (type, input) =>
                                                                                                (TOutput)
                                                                                                op.GetNamedClosure(type)(ctx,
                                                                                                                input);

                                                                                        }));
            return new PipelineConfigurator<TInput, TOutput>(definition);
        }

        public PipelineConfigurator<TInput, TOutput> CreatePipeline<TInput, TOutput>() 
        {
            var definition = new PipeDefinition(_registrationActions);
            var compiler = new PipelineCompiler(definition);

            _aggregatePipelines.Add(new PipelineKey
            {
                Input = typeof(TInput),
                Output = typeof(TOutput),
                Named = new DefaultPipeline<TInput, TOutput>()
            }, compiler);

            _registrationActions.Add(a => a.Register<Pipe<TInput, TOutput>>(ctx =>
                                                                                {
                                                                                    var compiledFunction =
                                                                                        compiler.Compile();

                                                                                    return
                                                                                        input =>
                                                                                            {
                                                                                                var output =
                                                                                                    compiledFunction(
                                                                                                        ctx, input);
                                                                                                return (TOutput) output;
                                                                                            };
                                                                                }
                                              ));

            return new PipelineConfigurator<TInput, TOutput>(definition);
        }

    }

}
