using System;
using System.Collections.Generic;
using System.Linq;
using Manifold.Configuration;
using Manifold.Configuration.Projector;
using Manifold.DependencyInjection;

namespace Manifold.Projector
{
    public class ProjectorCompiler<TInput,TOutput> : IPipeCompiler
    {
        private readonly IProjectorDefinition<TInput,TOutput> _projectorDefinition;

        public ProjectorCompiler(IProjectorDefinition<TInput, TOutput> projectorDefinition)
        {
            _projectorDefinition = projectorDefinition;
        }

        public Func<IPipelineContext, object, object> Compile()
        {
            return (injector, input) => from p in _projectorDefinition.Operations
                                        from q in p.GetExecutor()(injector, (TInput) input)
                                        select q;
        }

        public Func<IPipelineContext, TInput, IEnumerable<TOutput>> TypedCompile()
        {
            return (injector, input) => from p in _projectorDefinition.Operations
                                        from q in p.GetExecutor()(injector, input)
                                        select q;
        }
    }
}
