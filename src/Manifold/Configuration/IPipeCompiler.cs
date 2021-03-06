using System;
using Manifold.DependencyInjection;

namespace Manifold.Configuration
{
    public interface IPipeCompiler
    {
        Func<IPipelineContext, object, object> Compile();
    }
}