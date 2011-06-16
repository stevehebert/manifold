using System;
using Autofac;
using Wormhole.DependencyInjection;

namespace Wormhole.Autofac.Configuration
{
    public class TypeResolver : IResolveTypes
    {
        private readonly IComponentContext _componentContext;

        public TypeResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public object Resolve(Type type)
        {
            return _componentContext.Resolve(type);
        }
    }
}