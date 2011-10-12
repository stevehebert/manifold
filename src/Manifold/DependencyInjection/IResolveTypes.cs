using System;

namespace Manifold.DependencyInjection
{
    public interface IResolveTypes
    {
        object Resolve(Type type);
    }
}
