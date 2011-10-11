using System;
using Wormhole.DependencyInjection;

namespace Wormhole.Configuration.Operations
{
    public interface IOperation
    {
        Func<IResolveTypes, object, object> GetClosure();
    }

    public interface IRoutedOperation : IOperation
    {
        Func<IResolveTypes, object, bool> GetDecider();
    }
}
