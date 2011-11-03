﻿using System;
using System.Collections.Generic;
using System.Linq;
using Manifold.Configuration;
using Manifold.Configuration.Pipeline;
using Manifold.Configuration.Pipeline.Operations;
using Manifold.Configuration.Projector;
using Manifold.PipeAndFilter;
using Manifold.Projector;

namespace Manifold.DependencyInjection
{
    public class PipelineAggregator<TResolver> : IPipelineAggregator where TResolver : ITypeResolver
    {
        public PipelineAggregator()
        {
            _registrationActions.Add(a => a.RegisterResolver<TResolver>());
        }
        public IDictionary<PipelineKey, Func<IPipelineContext, object, object>> Compile(IRegisterTypes typeRegistrar)
        {
            var dictionary = _aggregatePipelines.ToDictionary(value => value.Key, value => value.Value.Compile()) as IDictionary<PipelineKey, Func<IPipelineContext, object, object>>;
            typeRegistrar.RegisterInstance(dictionary);

            typeRegistrar.RegisterType<PipelineContext, IPipelineContext>();
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
            var compiler = new PipelineCompiler<TInput, TOutput>(definition);

            _aggregatePipelines.Add(new PipelineKey
                                        {
                                            Input = typeof (TInput),
                                            Output = typeof (TOutput),
                                            Named = name
                                        }, compiler);

            _registrationActions.Add(a => a.Register(ctx => new NamedPipe<TType, TInput, TOutput>(name, compiler.TypedCompile())));
            
            _registrationActions.Add(a => a.Register<Pipe<TType, TInput, TOutput>>(ctx =>
                                                                                       {
                                                                                           var pipeThings = ctx.TypeResolver.Resolve(typeof(IEnumerable<NamedPipe<TType, TInput,TOutput>>)) as IEnumerable<NamedPipe<TType,TInput,TOutput>>; 

                                                                                            return (myname, input) =>
                                                                                            {
                                                                                                var pipeThing = (from p in pipeThings where p.Name.Equals(myname) select p).First();
                                                                                                return
                                                                                                    pipeThing.Pipe(
                                                                                                        ctx, input);


                                                                                            };
                                                                                       }));
            return new PipelineConfigurator<TInput, TOutput>(definition);
        }

        public PipelineConfigurator<TInput, TOutput> CreatePipeline<TInput, TOutput>() 
        {
            var definition = new PipeDefinition(_registrationActions);
            var compiler = new PipelineCompiler<TInput, TOutput>(definition);

            _aggregatePipelines.Add(new PipelineKey
            {
                Input = typeof(TInput),
                Output = typeof(TOutput),
                Named = new DefaultPipeline<TInput, TOutput>()
            }, compiler);

            _registrationActions.Add(a => a.Register(ctx => new AnonymousPipe<TInput, TOutput>(compiler.TypedCompile())));
            _registrationActions.Add(a => a.Register<Pipe<TInput, TOutput>>(ctx =>
                                                                                {
                                                                                    var pipe = ctx.TypeResolver.Resolve(typeof(AnonymousPipe<TInput, TOutput>)) as AnonymousPipe<TInput, TOutput>;

                                                                                    return input => pipe.Pipe(ctx, input);
                                                                                }));

            return new PipelineConfigurator<TInput, TOutput>(definition);
        }

        public ProjectorConfigurator<TInput, TOutput> CreateProjector<TInput, TOutput>()
        {
            var definition = new ProjectorDefinition<TInput, TOutput>(_registrationActions);
            var compiler = new ProjectorCompiler<TInput, TOutput>(definition);

            _aggregatePipelines.Add(new PipelineKey
            {
                Input = typeof(TInput),
                Output = typeof(IEnumerable<TOutput>),
                Named = new DefaultPipeline<TInput, IEnumerable<TOutput>>()
            }, compiler);

            _registrationActions.Add(a => a.Register(ctx => new AnonymousPipe<TInput, IEnumerable<TOutput>>(compiler.TypedCompile())));
            _registrationActions.Add(a => a.Register<Pipe<TInput, IEnumerable<TOutput>>>(ctx =>
            {
                var pipe = ctx.TypeResolver.Resolve(typeof(AnonymousPipe<TInput, IEnumerable<TOutput>>)) as AnonymousPipe<TInput, IEnumerable<TOutput>>;

                return input => pipe.Pipe(ctx, input);
            }));

            return new ProjectorConfigurator<TInput, TOutput>(definition);
        }

        public ProjectorConfigurator<TInput,TOutput> CreateProjector<TName, TInput, TOutput>(TName name)
        {
            var definition = new ProjectorDefinition<TInput, TOutput>(_registrationActions);
            var compiler = new ProjectorCompiler<TInput, TOutput>(definition);

            _aggregatePipelines.Add(new PipelineKey
            {
                Input = typeof(TInput),
                Output = typeof(IEnumerable<TOutput>),
                Named = name
            }, compiler);

            _registrationActions.Add(a => a.Register(ctx => new NamedPipe<TName, TInput, IEnumerable<TOutput>>(name, compiler.TypedCompile())));
            _registrationActions.Add(a => a.Register<Pipe<TName, TInput, IEnumerable<TOutput>>>( ctx =>
                                        {
                                            var pipeThings =
                                                ctx.TypeResolver.Resolve
                                                    (typeof (

                                                            IEnumerable<NamedPipe
                                                            <TName,
                                                            TInput,
                                                            IEnumerable<TOutput>>>)) as IEnumerable<NamedPipe<TName,TInput,IEnumerable<TOutput>>>;

                                            return (myname, input) =>
                                                {
                                                    var pipeThing = (from p in pipeThings where p.Name.Equals(myname) select p).First();
                                                    return
                                                        pipeThing.Pipe(
                                                            ctx, input);



                                                };}));
            return new ProjectorConfigurator<TInput, TOutput>(definition);
        }

        
    }
}
