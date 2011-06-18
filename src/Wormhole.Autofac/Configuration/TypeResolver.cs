using System;
using Autofac;
using Wormhole.DependencyInjection;

namespace Wormhole.Autofac.Configuration
{
    public class TypeResolver : IResolveTypes
    {
        private readonly IComponentContext _componentContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolver"/> class.
        /// </summary>
        /// <param name="componentContext">The autofac component context.</param>
        public TypeResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return _componentContext.Resolve(type);
        }
    }
}