using System;
using System.Collections.Generic;
using Autofac;
using Wormhole.Autofac.Configuration;
using Wormhole.DependencyInjection;
using Wormhole.Pipeline;

namespace Wormhole.Autofac
{
    public class PipelineModule : Module
    {
        private readonly IList<Action<ContainerBuilder>> _builderActions = new List<Action<ContainerBuilder>>();

        private readonly IList<Action<IDictionary<PipelineKey, Func<IResolveTypes, object, object>>>> _dictionaryActions
            = new List<Action<IDictionary<PipelineKey, Func<IResolveTypes, object, object>>>>();

        private readonly IDictionary<PipelineKey, Func<IResolveTypes, object, object>> _pipelineDictionary =
            new Dictionary<PipelineKey, Func<IResolveTypes, object, object>>();
        
        protected override void Load(ContainerBuilder builder)
        {
            foreach (var action in _dictionaryActions)
                action(_pipelineDictionary);

            foreach (var action in _builderActions)
                action(builder);

            builder.RegisterGeneric(typeof(NamedResolver<,>));

            builder.RegisterGeneric(typeof (Pipeline<,>)).As(typeof(IPipeline<,>));
            builder.Register(ctx => _pipelineDictionary);

            builder.RegisterType<TypeResolver>().As<IResolveTypes>();
            
            base.Load(builder);
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
            where TInput : class
            where TOutput : class
        {
            var configurator = RegisterPipeline<TInput, TOutput, DefaultPipeline<TInput,TOutput>>(new DefaultPipeline<TInput, TOutput>());

            _builderActions.Add(b =>
                                    {

                                        var compiledFunction =
                                            _pipelineDictionary[
                                                new PipelineKey
                                                    {
                                                        Input = typeof (TInput),
                                                        Output = typeof (TOutput),
                                                        Named = new DefaultPipeline<TInput, TOutput>()
                                                    }];

                                        b.Register<Func<TInput, TOutput>>(ctx =>
                                                                              {
                                                                                  var container = new TypeResolver( ctx.Resolve<IComponentContext>());

                                                                                  return a => (compiledFunction(container, a) as TOutput);
                                                                              });
                                    });
            return configurator;
        }

        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput, TNameType>(TNameType name)
            where TInput : class 
            where TOutput : class
        {
            var registrationData = new PipelineData();

            var items = new List<Action<IRegisterTypes>>();

            _builderActions.Add(builder =>
                                    {
                                        foreach (var item in items)
                                            item(new TypeRegistrar(builder));
                                    });



            _dictionaryActions.Add(
                a => a.Add(new PipelineKey {Input = typeof (TInput), Output = typeof (TOutput), Named = name},
                           PipelineCompiler.Compile(registrationData)));

            return new PipelineConfigurator<TInput, TOutput>(registrationData, items);
        }

    }
}
