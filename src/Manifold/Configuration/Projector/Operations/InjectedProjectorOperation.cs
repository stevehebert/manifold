using System;
using System.Collections.Generic;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Projector.Operations
{
    public class InjectedProjectorOperation<TType, TInput, TOutput> 
        : IProjectorOperation<TInput, TOutput> 
          where TType : class, IPipelineTask<TInput, IEnumerable<TOutput>>
    {
        public Func<IPipelineContext, TInput, IEnumerable<TOutput>> GetExecutor()
        {
            return (injector, o) =>
            {
                var item = (TType)injector.TypeResolver.Resolve(typeof(TType));

                if (item == null) throw new InvalidOperationException();

                return item.Execute(o);
            };
        }
    }
}
