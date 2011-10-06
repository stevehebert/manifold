using System;

namespace FormulaRossa
{
    public delegate object Injector(Type type);

    public interface IOperation
    {
        Func<Injector, object, object> GetClosure();
    }

    public class InjectedOperation<TType, TInput, TOutput> : IOperation where TType : IPipelineTask<TInput, TOutput>
    {
        public Func<Injector, object, object> GetClosure()
        {
            return (injector, o) =>
                       {
                           var item = (TType) injector(typeof (TType));

                           if (item == null) throw new InvalidOperationException();

                           var incoming = (TInput) o;

                           return item.Execute(incoming);
                       };
        }
    }
}
