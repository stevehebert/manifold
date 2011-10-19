using System;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Pipeline.Operations
{
    public class InjectedOperation<TType, TInput, TOutput> : IOperation where TType : IPipelineTask<TInput, TOutput>
    {
        public Func<IResolveTypes, object, object> GetExecutor()
        {
            return (injector, o) =>
            {
                var item = (TType) injector.Resolve(typeof(TType));

                if (item == null) throw new InvalidOperationException();

                var incoming = (TInput)o;

                return item.Execute(incoming);
            };
        }
    }
}
