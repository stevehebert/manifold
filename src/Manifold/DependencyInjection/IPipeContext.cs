using System;

namespace Manifold.DependencyInjection
{
    public interface IRegisterTypes
    {
        /// <summary>
        /// Registers the type.
        /// </summary>
        /// <typeparam name="TType">The type of the registration type.</typeparam>
        void RegisterType<TType>(bool asSingleton = false);

        /// <summary>
        /// Registers an instance with the container
        /// </summary>
        /// <typeparam name="TType">The type of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        void RegisterInstance<TType>(TType instance);

        /// <summary>
        /// Registers the generic.
        /// </summary>
        /// <param name="genericType">Type of the generic.</param>
        void RegisterGeneric(Type genericType);

        /// <summary>
        /// Registers a type, resolvable by another type
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <typeparam name="TAs">The type of as.</typeparam>
        void RegisterType<TType, TAs>();

        /// <summary>
        /// Registers the specified function.
        /// </summary>
        /// <typeparam name="TType">The type of the instance.</typeparam>
        /// <param name="function">The resolution function.</param>
        void Register<TType>(Func<IResolveTypes, TType> function);
    }
}
