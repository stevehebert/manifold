using System;
using Wormhole.DependencyInjection;

namespace Wormhole.Configuration
{
    public interface IPipeCompiler
    {
        Func<IResolveTypes, object, object> Compile();
    }
}