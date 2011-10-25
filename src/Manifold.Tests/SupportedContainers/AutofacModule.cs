using System;
using Autofac;
using Manifold.Autofac;
using Manifold.Configuration;

namespace Manifold.Tests.SupportedContainers
{
    public class AutofacModule : PipelineModule, ICommonModule
    {
        private readonly Action<IPipeCreator> _pipelineCreator;

        private readonly Lazy<IContainer> _container;

        public AutofacModule(Action<IPipeCreator> pipelineCreator)
        {
            _container = new Lazy<IContainer>(() =>
                                                  {
                                                      var containerBuilder =
                                                          new ContainerBuilder();
                                                      containerBuilder.RegisterModule(this);

                                                      return containerBuilder.Build();
                                                  });
            _pipelineCreator = pipelineCreator;
        }

        public override void RegisterPipelines(IPipeCreator pipeCreator)
        {
            _pipelineCreator(pipeCreator);
        }

        
        public TType Resolve<TType>()
        {
            return _container.Value.Resolve<TType>();
        }

        public void Build()
        {
            if(_container.Value == null) throw new InvalidOperationException();
        }
    }
}
