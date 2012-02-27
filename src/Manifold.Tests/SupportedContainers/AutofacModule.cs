﻿using System;
using System.Collections.Generic;
using Autofac;
using Manifold.Autofac;
using Manifold.Configuration;

namespace Manifold.Tests.SupportedContainers
{
    public class AutofacModule : PipelineModule, ICommonModule
    {
        private readonly Action<IPipeCreator> _pipelineCreator;
        private readonly ContainerBuilder _containerBuilder;
        private readonly Lazy<IContainer> _container;
        private readonly IList<AutofacModule> _autofacModules = new List<AutofacModule>(); 

        public AutofacModule(Action<IPipeCreator> pipelineCreator)
        {
            _containerBuilder = new ContainerBuilder();
            _containerBuilder.RegisterModule(this);

            foreach (var item in _autofacModules)
                _containerBuilder.RegisterModule(item);

            _container = new Lazy<IContainer>(() => _containerBuilder.Build());
            _pipelineCreator = pipelineCreator;
        }

        public void AddAlternateModules(IEnumerable<AutofacModule> modules)
        {
            foreach(var module in modules)
                _autofacModules.Add(module);
        }
        public override void RegisterPipelines(IPipeCreator pipeCreator)
        {
            _pipelineCreator(pipeCreator);
        }
        public void Register<TType, TInterface>() where TType : TInterface
        {
            _containerBuilder.RegisterType<TType>().As<TInterface>();
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
