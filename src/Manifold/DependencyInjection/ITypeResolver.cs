using System;

namespace Manifold.DependencyInjection
{
    public interface ITypeResolver
    {
        object Resolve(Type type);
    }
}