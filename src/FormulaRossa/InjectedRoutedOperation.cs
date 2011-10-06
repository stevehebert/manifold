using System;

namespace FormulaRossa
{
    public class InjectedRoutedOperation<TType, TInput, TOuput>: IOperation where TType : class, IRoutedPipelineTask<TInput, TOuput>
    {
        public Func<Injector, object, bool> GetDecider()
        {
            return (injector, o) =>
                       {
                           var item = (TType) injector(typeof (TType));

                           if (item == null) throw new InvalidOperationException();

                           return item.CanProcess((TInput) o);
                       };
        }

        public Func<Injector, object, object> GetClosure()
        {
            return (injector, o) =>
                       {
                           var item = (TType)injector(typeof(TType));

                           if (item == null) throw new InvalidOperationException();

                           var incoming = (TInput)o;

                           return item.Execute(incoming);
                       };
        }
    }
}