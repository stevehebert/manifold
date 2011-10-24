using System;
using System.Collections.Generic;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Projector.Operations
{
    public interface IProjectorOperation<in TInput, out TOutput>
    {
        Func<IPipelineContext, TInput, IEnumerable<TOutput>> GetExecutor();
    }
}
