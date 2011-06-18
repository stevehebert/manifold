using System;

namespace Wormhole.DependencyInjection
{
    public interface IRegisterTypes
    {
        void RegisterType<TType>();
        void RegisterType<TType>(Func<IResolveTypes, TType> function);
        void RegisterInstance<TType>(TType instance);
        void RegisterGeneric(Type genericType);
        void RegisterType<TType, TAs>();

        void Register<TType>(Func<IResolveTypes, TType> function);
    }
}
