using System;

namespace Wormhole.DependencyInjection
{
    public class InjectedRoutedOperation<TType, TInput, TOutput> : IRoutedOperation where TType : class, IRoutedPipelineTask<TInput, TOutput>
    {
        public Func<IResolveTypes, object, object> GetClosure()
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
