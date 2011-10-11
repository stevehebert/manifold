using System;
using System.Collections.Generic;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline.Configuration
{
    public class PipelineAggregator<TResolver> where TResolver : IResolveTypes
    {
        private readonly IList<Action<IDictionary<PipelineKey, Func<Tuple<IResolveTypes, object>, object, object, object>>>> _dictionaryActions
            = new List<Action<IDictionary<PipelineKey, Func<Tuple<IResolveTypes, object>, object, object, object>>>>();

        private readonly IDictionary<PipelineKey, Func<Tuple<IResolveTypes, object>, object, object, object>> _pipelineDictionary
            = new Dictionary<PipelineKey, Func<Tuple<IResolveTypes, object>, object, object, object>>();

        private readonly IList<Action<IRegisterTypes>> _registrationActions = new List<Action<IRegisterTypes>>();

        public PipelineAggregator()
        {
            _registrationActions.Add(a => a.RegisterInstance(_pipelineDictionary));
            _registrationActions.Add(a => a.RegisterGeneric(typeof(NamedResolver<,>)));
            _registrationActions.Add(a => a.RegisterType<TResolver, IResolveTypes>());
            _registrationActions.Add(a => a.RegisterType<PipelineDataResolver>(true));
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
        {
            var configurator =
                RegisterPipeline<DefaultPipeline<TInput, TOutput>, TInput, TOutput>(
                    new DefaultPipeline<TInput, TOutput>());

            _registrationActions.Add(a =>
                                         {
                                             var pipelineKey = new PipelineKey
                                                                   {
                                                                       Input = typeof(TInput),
                                                                       Output = typeof(TOutput),
                                                                       Named = new DefaultPipeline<TInput, TOutput>()
                                                                   };

                                             var compiledFunction =
                                                 _pipelineDictionary[
                                                     pipelineKey];

                                             a.Register<Pipe<TInput, TOutput>>(
                                                 c => input => (TOutput)compiledFunction(new Tuple<IResolveTypes, object>(c, pipelineKey.Named), pipelineKey.Named,  input));
                                         });

            return configurator;
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name)
        {
            var registrationData = new PipelineData();

            _dictionaryActions.Add(
                a => a.Add(new PipelineKey { Input = typeof(TInput), Output = typeof(TOutput), Named = name },
                           PipelineCompiler.Compile(registrationData)));

            if (typeof(TNameType) != typeof(DefaultPipeline<TInput, TOutput>))
            {
                _registrationActions.Add(a => a.Register<Pipe<TNameType, TInput, TOutput>>(c =>
                                            {
                                                var item =
                                                    c.Resolve(typeof(NamedResolver<TInput, TOutput>)) as
                                                    NamedResolver<TInput, TOutput>;

                                                return
                                                    (clarifier, input) => item.Execute(clarifier, input);
                                            }));
            }

            return new PipelineConfigurator<TInput, TOutput>(registrationData, _registrationActions);
        }

        public void PerformRegistration(IRegisterTypes typeRegistrar)
        {
            // first, we'll hydrate the pipeline dictionary
            foreach (var action in _dictionaryActions)
                action(_pipelineDictionary);

            // next we'll register our pending actions
            foreach (var item in _registrationActions)
                item(typeRegistrar);
        }
    }
}
