using System;
using Wormhole.DependencyInjection;

namespace Wormhole.PipeAndFilter
{
    public interface IPipeCompiler
    {
        Func<IResolveTypes, object, object> Compile();
    }
}