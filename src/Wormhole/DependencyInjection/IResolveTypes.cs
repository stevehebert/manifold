using System;

namespace Wormhole.DependencyInjection
{
    public interface IResolveTypes
    {
        object Resolve(Type type);
    }
}
