using System;
using Autofac;
using Wormhole.DependencyInjection;

namespace Wormhole.Autofac.Configuration
{
    public class TypeRegistrar : IRegisterTypes
    {
        private readonly ContainerBuilder _containerBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistrar"/> class.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        public TypeRegistrar(ContainerBuilder containerBuilder )
        {
            _containerBuilder = containerBuilder;
        }

        /// <summary>
        /// Registers the provided type with the underlying container
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        public void RegisterType<TType>()
        {
            _containerBuilder.RegisterType<TType>();
        }
    }
}
