using System;

namespace Wormhole.DependencyInjection
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
