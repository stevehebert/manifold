using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manifold.Configuration;
using Manifold.DependencyInjection;
using Manifold.Ninject.Configuration;
using Ninject.Modules;

namespace Manifold.Ninject
{
    public abstract class PipelineModule : NinjectModule 
    {
        private readonly IPipeCreator _pipeCreator;

        protected PipelineModule()
            : base()
        {
            _pipeCreator = new PipeCreator(new PipelineAggregator<NinjectTypeResolver>());
        }

        public abstract void RegisterPipelines(IPipeCreator pipeCreator);

        public override void Load()
        {
            RegisterPipelines(_pipeCreator);
            _pipeCreator.Compile(new NinjectTypeRegistrar(this));
        }
    }
}
