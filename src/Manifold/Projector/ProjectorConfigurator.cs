using System;
using System.Collections.Generic;
using Manifold.Configuration;
using Manifold.Configuration.Projector;

namespace Manifold.Projector
{
    public class ProjectorConfigurator<TInput, TOutput>
    {
        private readonly IProjectorDefinition<TInput,TOutput> _projectorDefinition;

        public ProjectorConfigurator(IProjectorDefinition<TInput, TOutput> projectorDefiniton)
        {
            _projectorDefinition = projectorDefiniton;
        }

        public ProjectorConfigurator<TInput, TOutput> Bind<TType>() where TType : class, IProjectorTask<TInput,TOutput>
        {
            _projectorDefinition.AddInjectedOperation<TType>();
            return this;
        }
        public ProjectorConfigurator<TInput, TOutput> Bind(Func<TInput, IEnumerable<TOutput>> function )
        {
            _projectorDefinition.AddFunctionOperation(function);

            return this;
        }
    }

}
