using System;
using Wormhole.DependencyInjection;

namespace Wormhole.Configuration.Operations
{
    public class InjectedRoutedOperation<TType, TInput, TOutput> : IRoutedOperation where TType : class, IRoutingPipelineTask<TInput, TOutput>
    {
        public Func<IResolveTypes, object, object> GetExecutor()
        {
            return (injector, o) =>
                       {
                           var item = (TType) injector.Resolve(typeof (TType));

                           if (item == null) throw new InvalidOperationException();

                           var incoming = (TInput) o;

                           return item.Execute(incoming);
                       };
        }

        public Func<IResolveTypes, object, bool> GetDecider()
        {
            return (injector, o) =>
                       {
                           var item = (TType) injector.Resolve(typeof (TType));

                           if (item == null) throw new InvalidOperationException();

                           var incoming = (TInput) o;
                           return item.CanExecute(incoming);
                       };
        }
    }
}
