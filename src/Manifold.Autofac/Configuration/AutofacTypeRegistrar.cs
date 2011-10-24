 using System;
using Autofac;
using Manifold.DependencyInjection;

namespace Manifold.Autofac.Configuration
{
    public class AutofacTypeRegistrar : IRegisterTypes
    {
        private readonly ContainerBuilder _containerBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacTypeRegistrar"/> class.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        public AutofacTypeRegistrar(ContainerBuilder containerBuilder )
        {
            _containerBuilder = containerBuilder;
        }

        /// <summary>
        /// Registers the provided type with the underlying container
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        public void RegisterType<TType>(bool asSingleton = false)
        {
            var registration = _containerBuilder.RegisterType<TType>();

            if (asSingleton)
                registration.SingleInstance();
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance<TType>(TType instance)
        {
            _containerBuilder.Register(c => instance);
        }

        /// <summary>
        /// Registers the generic.
        /// </summary>
        /// <param name="genericType">Type of the generic.</param>
        public void RegisterGeneric(Type genericType)
        {
            _containerBuilder.RegisterGeneric(genericType);
        }

        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <typeparam name="TAs">The type of as.</typeparam>
        public void RegisterType<TType, TAs>()
        {
            _containerBuilder.RegisterType<TType>().As<TAs>();
        }

        /// <summary>
        /// Registers the specified function.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="function">The function.</param>
        public void Register<TType>(Func<IPipelineContext, TType> function)
        {
            _containerBuilder.Register(ctx => function(ctx.Resolve<IPipelineContext>()));
        }
    }
}
