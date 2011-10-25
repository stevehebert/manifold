using System;
using Manifold.DependencyInjection;
using Ninject.Activation;

namespace Manifold.Ninject.Configuration
{
    public class NinjectTypeRegistrar : IRegisterTypes  
    {
        private readonly PipelineModule _pipelineModule;

        public NinjectTypeRegistrar(PipelineModule pipelineModule)
        {
            _pipelineModule = pipelineModule;
        }

        public void RegisterType<TType>(bool asSingleton = false)
        {
            if( asSingleton)
                _pipelineModule.Bind<TType>().ToSelf().InSingletonScope();
            else
                _pipelineModule.Bind<TType>().ToSelf().InTransientScope();
        }

        public void RegisterInstance<TType>(TType instance)
        {
            _pipelineModule.Bind<TType>().ToMethod(ctx => instance);
        }

        public void RegisterGeneric(Type genericType)
        {
            _pipelineModule.Bind(genericType).To(genericType);
        }

        public void RegisterType<TType, TAs>() where TType : TAs
        {
            _pipelineModule.Bind<TAs>().To<TType>();
        }

        public void Register<TType>(Func<IPipelineContext, TType> function)
        {
            _pipelineModule.Bind<TType>().ToProvider(new FuncProvider<TType>(function));
        }

        public void RegisterResolver<TType>() where TType : ITypeResolver
        {
            _pipelineModule.Bind<ITypeResolver>().ToProvider(new ResolverProvider<TType>());
        }
    }

    public class ResolverProvider<TType> : IProvider
    {
        public object Create(IContext context)
        {
            return new NinjectTypeResolver(context.Kernel);
        }

        public Type Type
        {
            get { return typeof (TType); }
        }
    }
}
