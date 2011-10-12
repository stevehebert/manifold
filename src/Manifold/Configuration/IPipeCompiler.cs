using System;
using Manifold.DependencyInjection;

namespace Manifold.Configuration
{
    public interface IPipeCompiler
    {
        Func<IResolveTypes, object, object> Compile();
    }
}