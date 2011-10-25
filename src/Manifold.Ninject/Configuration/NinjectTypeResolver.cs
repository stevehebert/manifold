using System;
using Manifold.DependencyInjection;
using Ninject;

namespace Manifold.Ninject.Configuration
{
    public class NinjectTypeResolver : ITypeResolver 
    {
        private readonly IKernel _kernel;

        public NinjectTypeResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object Resolve(Type type)
        {
            return _kernel.Get(type);
        }
    }
}
