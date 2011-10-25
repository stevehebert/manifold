using System;
using Manifold.DependencyInjection;
using Ninject;
using Ninject.Activation;

namespace Manifold.Ninject.Configuration
{
    public class FuncProvider<T> : IProvider
    {
        private readonly Func<IPipelineContext, T> _function;

        public FuncProvider(Func<IPipelineContext, T> function)
        {
            _function = function;
        }

        public object Create(IContext context)
        {
            var ctx = context.Kernel.Get<IPipelineContext>();

            return _function(ctx);
            //return ctx.TypeResolver.Resolve(typeof (T));
        }

        public Type Type
        {
            get { return typeof(T); }
        }
    }
}