using System;

namespace FormulaRossa
{
    public delegate object Injector(Type type);

    public interface IOperation
    {
        Func<Injector, object, object> GetClosure();
    }

    public interface IRoutedOperation : IOperation
    {
        Func<Injector, object, bool> GetDecider();
    }
}
