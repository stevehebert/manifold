using System;
using Manifold.Configuration;
using Manifold.Ninject;
using Ninject;

namespace Manifold.Tests.SupportedContainers
{
    public class NinjectModule : PipelineModule, ICommonModule
    {
        private readonly Action<IPipeCreator> _pipelineCreator;

        private readonly Lazy<IKernel> _kernel; 

        public NinjectModule(Action<IPipeCreator> pipelineCreator)
        {
            _pipelineCreator = pipelineCreator;
            _kernel = new Lazy<IKernel>(() =>
                                            {
                                                var kernel = new StandardKernel();
                                                kernel.Load(this);
                                                return kernel;
                                            });
        }

        public override void RegisterPipelines(IPipeCreator pipeCreator)
        {
            _pipelineCreator(pipeCreator);
        }

        public TType Resolve<TType>()
        {
            return _kernel.Value.Get<TType>();
        }

        public void Build()
        {
            if (_kernel.Value == null) throw new InvalidOperationException();
        }
    }
}
