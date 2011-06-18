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

        public void RegisterType<TType>(Func<IResolveTypes, TType> function)
        {
            Register(function);
        }

        public void RegisterInstance<TType>(TType instance)
        {
            _containerBuilder.Register(c => instance);
        }

        public void RegisterGeneric(Type genericType)
        {
            _containerBuilder.RegisterGeneric(genericType);
        }

        public void RegisterType<TType, TAs>()
        {
            _containerBuilder.RegisterType<TType>().As<TAs>();
        }

        public void Register<TType>(Func<IResolveTypes, TType> function)
        {
            _containerBuilder.Register(ctx => function(ctx.Resolve<IResolveTypes>()));
        }
    }
}
