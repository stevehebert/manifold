using System;
using System.Collections.Generic;
using Manifold.Configuration.Projector.Operations;

namespace Manifold.Configuration.Projector
{
    public interface IProjectorDefinition<TInput, TOutput>
    {
        IEnumerable<IProjectorOperation<TInput, TOutput>> Operations { get; }

        void AddInjectedOperation<TType>() where TType : class, IPipelineTask<TInput, IEnumerable<TOutput>>;
        void AddFunctionOperation(Func<TInput, IEnumerable<TOutput>> function);
    }
}