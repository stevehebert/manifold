using System;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Pipeline.Operations
{
    public interface IOperation
    {
        Func<IPipelineContext, object, object> GetExecutor();
    }

    public interface IRoutedOperation : IOperation
    {
        Func<IPipelineContext, object, bool> GetDecider();
    }
}
