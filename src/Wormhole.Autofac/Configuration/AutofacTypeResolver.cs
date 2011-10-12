using System;
using Autofac;
using Manifold.DependencyInjection;

namespace Manifold.Autofac.Configuration
{
    public class AutofacTypeResolver : IResolveTypes
    {
        private readonly IComponentContext _componentContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacTypeResolver"/> class.
        /// </summary>
        /// <param name="componentContext">The autofac component context.</param>
        public AutofacTypeResolver(IComponentContext componentContext)
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